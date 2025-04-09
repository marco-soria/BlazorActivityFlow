using ActivityFlow.Shared.Models;

namespace ActivityFlow.Server.Services;

public interface IUserService
{
    Task<List<UserDto>> GetAllUsersAsync();
    Task<UserDto?> GetUserByIdAsync(string userId);
    Task<AuthResponse> CreateUserAsync(CreateUserRequest request);
    Task<AuthResponse> UpdateUserAsync(UpdateUserRequest request);
    Task<AuthResponse> DeleteUserAsync(string userId);
} 