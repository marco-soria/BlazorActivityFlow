using ActivityFlow.Server.Models;
using ActivityFlow.Shared.Models;

namespace ActivityFlow.Server.Services;

public interface IActivityService
{
    Task<List<ActivityFlow.Server.Models.Activity>> GetAllActivitiesAsync();
    Task<ActivityFlow.Server.Models.Activity?> GetActivityByIdAsync(int id);
    Task<List<ActivityFlow.Server.Models.Activity>> GetActivitiesByUserIdAsync(string userId);
    Task<List<ActivityFlow.Server.Models.Activity>> GetActivitiesByCategoryIdAsync(int categoryId);
    Task<ActivityFlow.Server.Models.Activity> CreateActivityAsync(CreateActivityDto activityDto);
    Task<ActivityFlow.Server.Models.Activity> UpdateActivityAsync(int id, UpdateActivityDto activityDto);
    Task<bool> DeleteActivityAsync(int id);
} 