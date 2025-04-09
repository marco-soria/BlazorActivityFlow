using System.Net.Http.Json;
using ActivityFlow.Shared.Models;

namespace ActivityFlow.Client.Services;

public class ActivityService : IActivityService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ActivityService> _logger;

    public ActivityService(HttpClient httpClient, ILogger<ActivityService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<List<Activity>> GetAllActivitiesAsync()
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<List<Activity>>("api/activities");
            return response ?? new List<Activity>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener todas las actividades");
            throw;
        }
    }

    public async Task<Activity?> GetActivityByIdAsync(int id)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<Activity>($"api/activities/{id}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener la actividad con ID {Id}", id);
            throw;
        }
    }

    public async Task<List<Activity>> GetActivitiesByUserIdAsync(string userId)
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<List<Activity>>($"api/activities/user/{userId}");
            return response ?? new List<Activity>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener actividades del usuario {UserId}", userId);
            throw;
        }
    }

    public async Task<List<Activity>> GetActivitiesByCategoryIdAsync(int categoryId)
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<List<Activity>>($"api/activities/category/{categoryId}");
            return response ?? new List<Activity>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener actividades de la categoría {CategoryId}", categoryId);
            throw;
        }
    }

    public async Task<Activity> CreateActivityAsync(CreateActivityDto activityDto)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/activities", activityDto);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Activity>() 
                ?? throw new InvalidOperationException("No se pudo crear la actividad");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear la actividad");
            throw;
        }
    }

    public async Task<Activity> UpdateActivityAsync(int id, UpdateActivityDto activityDto)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync($"api/activities/{id}", activityDto);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Activity>() 
                ?? throw new InvalidOperationException("No se pudo actualizar la actividad");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar la actividad con ID {Id}", id);
            throw;
        }
    }

    public async Task<bool> DeleteActivityAsync(int id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"api/activities/{id}");
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar la actividad con ID {Id}", id);
            throw;
        }
    }
} 