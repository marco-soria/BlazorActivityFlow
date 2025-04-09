using ActivityFlow.Shared.Models;

namespace ActivityFlow.Client.Services;

public interface IActivityService
{
    Task<List<Activity>> GetAllActivitiesAsync();
    Task<Activity?> GetActivityByIdAsync(int id);
    Task<List<Activity>> GetActivitiesByUserIdAsync(string userId);
    Task<List<Activity>> GetActivitiesByCategoryIdAsync(int categoryId);
    Task<Activity> CreateActivityAsync(CreateActivityDto activityDto);
    Task<Activity> UpdateActivityAsync(int id, UpdateActivityDto activityDto);
    Task<bool> DeleteActivityAsync(int id);
} 