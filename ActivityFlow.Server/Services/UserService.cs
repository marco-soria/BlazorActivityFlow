using ActivityFlow.Shared.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ActivityFlow.Server.Services;

public class UserService : IUserService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ILogger<UserService> _logger;

    public UserService(UserManager<IdentityUser> userManager, ILogger<UserService> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<List<UserDto>> GetAllUsersAsync()
    {
        try
        {
            var users = await _userManager.Users
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    Email = u.Email ?? string.Empty,
                    UserName = u.UserName ?? string.Empty,
                    EmailConfirmed = u.EmailConfirmed,
                    LockoutEnabled = u.LockoutEnabled,
                    LockoutEnd = u.LockoutEnd
                })
                .ToListAsync();

            foreach (var user in users)
            {
                var identityUser = await _userManager.FindByIdAsync(user.Id);
                if (identityUser != null)
                {
                    user.Roles = (await _userManager.GetRolesAsync(identityUser)).ToList();
                }
            }

            return users;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener todos los usuarios");
            throw;
        }
    }

    public async Task<UserDto?> GetUserByIdAsync(string userId)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return null;

            var roles = await _userManager.GetRolesAsync(user);

            return new UserDto
            {
                Id = user.Id,
                Email = user.Email ?? string.Empty,
                UserName = user.UserName ?? string.Empty,
                Roles = roles.ToList(),
                EmailConfirmed = user.EmailConfirmed,
                LockoutEnabled = user.LockoutEnabled,
                LockoutEnd = user.LockoutEnd
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener usuario por ID: {UserId}", userId);
            throw;
        }
    }

    public async Task<AuthResponse> UpdateUserAsync(UpdateUserRequest request)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(request.Id);
            if (user == null)
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = "Usuario no encontrado"
                };
            }

            user.Email = request.Email;
            user.UserName = request.UserName;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = string.Join(", ", result.Errors.Select(e => e.Description))
                };
            }

            // Actualizar roles
            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);
            await _userManager.AddToRolesAsync(user, request.Roles);

            return new AuthResponse
            {
                Success = true,
                Message = "Usuario actualizado correctamente"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar usuario: {UserId}", request.Id);
            throw;
        }
    }

    public async Task<AuthResponse> DeleteUserAsync(string userId)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = "Usuario no encontrado"
                };
            }

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = string.Join(", ", result.Errors.Select(e => e.Description))
                };
            }

            return new AuthResponse
            {
                Success = true,
                Message = "Usuario eliminado correctamente"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar usuario: {UserId}", userId);
            throw;
        }
    }

    public async Task<AuthResponse> CreateUserAsync(CreateUserRequest request)
    {
        try
        {
            // Verificar si el usuario ya existe
            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser != null)
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = "El email ya está registrado"
                };
            }

            // Crear el usuario
            var user = new IdentityUser
            {
                UserName = request.UserName,
                Email = request.Email,
                EmailConfirmed = true // Para pruebas, en producción debería ser false
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = string.Join(", ", result.Errors.Select(e => e.Description))
                };
            }

            // Asignar roles
            if (request.Roles.Any())
            {
                result = await _userManager.AddToRolesAsync(user, request.Roles);
                if (!result.Succeeded)
                {
                    // Si falla la asignación de roles, eliminamos el usuario
                    await _userManager.DeleteAsync(user);
                    return new AuthResponse
                    {
                        Success = false,
                        Message = string.Join(", ", result.Errors.Select(e => e.Description))
                    };
                }
            }

            return new AuthResponse
            {
                Success = true,
                Message = "Usuario creado correctamente"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear usuario: {Email}", request.Email);
            throw;
        }
    }
} 