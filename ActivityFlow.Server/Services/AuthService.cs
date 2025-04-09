using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ActivityFlow.Server.Models;
using ActivityFlow.Shared.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Logging;

namespace ActivityFlow.Server.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthService> _logger;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        IConfiguration configuration,
        ILogger<AuthService> logger)
    {
        _userManager = userManager;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return new AuthResponse { Success = false, Message = "Usuario no encontrado" };
            }

            var result = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!result)
            {
                return new AuthResponse { Success = false, Message = "Contraseña incorrecta" };
            }

            var token = await GenerateJwtTokenAsync(user);
            return new AuthResponse
            {
                Success = true,
                Token = token,
                Message = "Login exitoso"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error durante el login");
            return new AuthResponse { Success = false, Message = "Error durante el login" };
        }
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        if (request.Password != request.ConfirmPassword)
        {
            return new AuthResponse
            {
                Success = false,
                Message = "Las contraseñas no coinciden"
            };
        }

        var user = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email
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

        // Asignar rol User por defecto
        result = await _userManager.AddToRoleAsync(user, "User");
        if (!result.Succeeded)
        {
            // Si falla la asignación del rol, eliminamos el usuario
            await _userManager.DeleteAsync(user);
            return new AuthResponse
            {
                Success = false,
                Message = "Error al asignar el rol por defecto"
            };
        }

        return await GenerateTokens(user);
    }

    public async Task<AuthResponse> RefreshTokenAsync(string refreshToken)
    {
        var user = await _userManager.FindByIdAsync(refreshToken);
        if (user == null)
        {
            return new AuthResponse
            {
                Success = false,
                Message = "Token de actualización inválido"
            };
        }

        return await GenerateTokens(user);
    }

    private async Task<AuthResponse> GenerateTokens(ApplicationUser user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
            new Claim(ClaimTypes.Name, user.UserName ?? string.Empty),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var roles = await _userManager.GetRolesAsync(user);
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"] ?? throw new InvalidOperationException("JWT Secret Key no configurado")));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.Now.AddMinutes(int.Parse(_configuration["JwtSettings:ExpirationInMinutes"] ?? "60"));

        var token = new JwtSecurityToken(
            issuer: _configuration["JwtSettings:Issuer"],
            audience: _configuration["JwtSettings:Audience"],
            claims: claims,
            expires: expires,
            signingCredentials: creds
        );

        return new AuthResponse
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            RefreshToken = user.Id,
            Expiration = expires,
            Success = true,
            Message = "Autenticación exitosa"
        };
    }

    private async Task<string> GenerateJwtTokenAsync(ApplicationUser user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
            new Claim(ClaimTypes.Name, user.UserName ?? string.Empty)
        };

        var roles = await _userManager.GetRolesAsync(user);
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"] ?? throw new InvalidOperationException("JWT Secret Key no configurado")));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["JwtSettings:ExpirationInDays"]));

        var token = new JwtSecurityToken(
            issuer: _configuration["JwtSettings:Issuer"],
            audience: _configuration["JwtSettings:Audience"],
            claims: claims,
            expires: expires,
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
} 