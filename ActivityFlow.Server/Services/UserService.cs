using ActivityFlow.Server.Data;
using ActivityFlow.Server.Models;
using ActivityFlow.Shared.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ActivityFlow.Server.Services;

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _context;
    private readonly ILogger<UserService> _logger;

    public UserService(
        UserManager<ApplicationUser> userManager,
        ApplicationDbContext context,
        ILogger<UserService> logger)
    {
        _userManager = userManager;
        _context = context;
        _logger = logger;
    }

    public async Task<List<UserDto>> GetAllUsersAsync()
    {
        var users = await _userManager.Users
            .Select(u => new UserDto
            {
                Id = u.Id,
                Email = u.Email ?? string.Empty,
                UserName = u.UserName ?? string.Empty,
                Roles = new List<string>(),
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

    public async Task<UserDto?> GetUserByIdAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return null;

        var roles = await _userManager.GetRolesAsync(user);

        return new UserDto
        {
            Id = user.Id,
            Email = user.Email,
            UserName = user.UserName,
            Roles = roles.ToList(),
            EmailConfirmed = user.EmailConfirmed,
            LockoutEnabled = user.LockoutEnabled,
            LockoutEnd = user.LockoutEnd
        };
    }

    public async Task<UserDto?> GetUserByEmailAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null) return null;

        var roles = await _userManager.GetRolesAsync(user);

        return new UserDto
        {
            Id = user.Id,
            Email = user.Email,
            UserName = user.UserName,
            Roles = roles.ToList(),
            EmailConfirmed = user.EmailConfirmed,
            LockoutEnabled = user.LockoutEnabled,
            LockoutEnd = user.LockoutEnd
        };
    }

    public async Task<AuthResponse> UpdateUserAsync(UpdateUserRequest request)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
            {
                return new AuthResponse { Success = false, Message = "Usuario no encontrado" };
            }

            user.Email = request.Email;
            user.UserName = request.Email;

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
            if (request.Roles.Any())
            {
                await _userManager.AddToRolesAsync(user, request.Roles);
            }

            return new AuthResponse { Success = true, Message = "Usuario actualizado exitosamente" };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar usuario");
            return new AuthResponse { Success = false, Message = "Error al actualizar usuario" };
        }
    }

    public async Task<AuthResponse> ChangePasswordAsync(ChangePasswordRequest request)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
            {
                return new AuthResponse { Success = false, Message = "Usuario no encontrado" };
            }

            var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
            if (!result.Succeeded)
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = string.Join(", ", result.Errors.Select(e => e.Description))
                };
            }

            return new AuthResponse { Success = true, Message = "Contraseña cambiada exitosamente" };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al cambiar contraseña");
            return new AuthResponse { Success = false, Message = "Error al cambiar contraseña" };
        }
    }

    public async Task<AuthResponse> ResetPasswordAsync(ResetPasswordRequest request)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return new AuthResponse { Success = false, Message = "Usuario no encontrado" };
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, request.NewPassword);
            if (!result.Succeeded)
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = string.Join(", ", result.Errors.Select(e => e.Description))
                };
            }

            return new AuthResponse { Success = true, Message = "Contraseña restablecida exitosamente" };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al restablecer contraseña");
            return new AuthResponse { Success = false, Message = "Error al restablecer contraseña" };
        }
    }

    public async Task<AuthResponse> LockUserAsync(string userId, DateTimeOffset? lockoutEnd)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new AuthResponse { Success = false, Message = "Usuario no encontrado" };
            }

            var result = await _userManager.SetLockoutEndDateAsync(user, lockoutEnd);
            if (!result.Succeeded)
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = string.Join(", ", result.Errors.Select(e => e.Description))
                };
            }

            return new AuthResponse { Success = true, Message = "Usuario bloqueado exitosamente" };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al bloquear usuario");
            return new AuthResponse { Success = false, Message = "Error al bloquear usuario" };
        }
    }

    public async Task<AuthResponse> UnlockUserAsync(string userId)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new AuthResponse { Success = false, Message = "Usuario no encontrado" };
            }

            var result = await _userManager.SetLockoutEndDateAsync(user, null);
            if (!result.Succeeded)
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = string.Join(", ", result.Errors.Select(e => e.Description))
                };
            }

            return new AuthResponse { Success = true, Message = "Usuario desbloqueado exitosamente" };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al desbloquear usuario");
            return new AuthResponse { Success = false, Message = "Error al desbloquear usuario" };
        }
    }

    public async Task<AuthResponse> DeleteUserAsync(string userId)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning("Usuario no encontrado: {UserId}", userId);
                return new AuthResponse { Success = false, Message = "Usuario no encontrado" };
            }

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                _logger.LogWarning("Error al eliminar usuario: {Errors}", string.Join(", ", result.Errors.Select(e => e.Description)));
                return new AuthResponse { Success = false, Message = "Error al eliminar usuario" };
            }

            _logger.LogInformation("Usuario eliminado exitosamente: {UserId}", userId);
            return new AuthResponse { Success = true, Message = "Usuario eliminado exitosamente" };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar usuario");
            return new AuthResponse { Success = false, Message = "Error al eliminar usuario" };
        }
    }

    public async Task<AuthResponse> CreateUserAsync(CreateUserRequest request)
    {
        try
        {
            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser != null)
            {
                return new AuthResponse { Success = false, Message = "El email ya está registrado" };
            }

            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                EmailConfirmed = true
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

            if (request.Roles.Any())
            {
                result = await _userManager.AddToRolesAsync(user, request.Roles);
                if (!result.Succeeded)
                {
                    await _userManager.DeleteAsync(user);
                    return new AuthResponse
                    {
                        Success = false,
                        Message = string.Join(", ", result.Errors.Select(e => e.Description))
                    };
                }
            }

            return new AuthResponse { Success = true, Message = "Usuario creado exitosamente" };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear usuario");
            return new AuthResponse { Success = false, Message = "Error al crear usuario" };
        }
    }
} 