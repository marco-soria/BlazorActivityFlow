using ActivityFlow.Server.Data;
using ActivityFlow.Server.Models;
using ActivityFlow.Shared.Models;
using ActivityFlow.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace ActivityFlow.Server.Services;

public class ActivityService : IActivityService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ActivityService> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ActivityService(
        ApplicationDbContext context,
        ILogger<ActivityService> logger,
        IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<List<ActivityFlow.Server.Models.Activity>> GetAllActivitiesAsync()
    {
        return await _context.Activities
            .Include(a => a.Category)
            .Include(a => a.Status)
            .ToListAsync();
    }

    public async Task<ActivityFlow.Server.Models.Activity?> GetActivityByIdAsync(int id)
    {
        return await _context.Activities
            .Include(a => a.Category)
            .Include(a => a.Status)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<List<ActivityFlow.Server.Models.Activity>> GetActivitiesByUserIdAsync(string userId)
    {
        return await _context.Activities
            .Include(a => a.Category)
            .Include(a => a.Status)
            .Where(a => a.UserId == userId)
            .ToListAsync();
    }

    public async Task<List<ActivityFlow.Server.Models.Activity>> GetActivitiesByCategoryIdAsync(int categoryId)
    {
        return await _context.Activities
            .Include(a => a.Category)
            .Include(a => a.Status)
            .Where(a => a.CategoryId == categoryId)
            .ToListAsync();
    }

    public async Task<ActivityFlow.Server.Models.Activity> CreateActivityAsync(CreateActivityDto activityDto)
    {
        try
        {
            if (_httpContextAccessor.HttpContext == null || !_httpContextAccessor.HttpContext.User.Identity?.IsAuthenticated == true)
            {
                _logger.LogWarning("Intento de crear actividad sin autenticación");
                throw new UnauthorizedAccessException("Usuario no autenticado");
            }

            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogError("No se pudo obtener el ID del usuario autenticado");
                throw new UnauthorizedAccessException("No se pudo identificar al usuario");
            }

            var category = await _context.Categories.FindAsync(activityDto.CategoryId);
            if (category == null)
            {
                _logger.LogWarning($"Categoría no encontrada: {activityDto.CategoryId}");
                throw new KeyNotFoundException($"No se encontró la categoría con ID {activityDto.CategoryId}");
            }

            // Obtener el estado "Pending" por defecto
            var defaultStatus = await _context.Statuses
                .FirstOrDefaultAsync(s => s.Name == ActivityStatus.Pending);

            if (defaultStatus == null)
            {
                _logger.LogError("No se encontró el estado Pending en la base de datos");
                throw new InvalidOperationException("No se pudo establecer el estado inicial de la actividad");
            }

            var activity = new ActivityFlow.Server.Models.Activity
            {
                Title = activityDto.Title,
                Description = activityDto.Description,
                CategoryId = activityDto.CategoryId,
                UserId = userId,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(7),
                StatusId = defaultStatus.Id,
                CreatedAt = DateTime.UtcNow
            };

            _context.Activities.Add(activity);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Actividad creada exitosamente: {activity.Id}");
            return activity;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear la actividad");
            throw;
        }
    }

    public async Task<ActivityFlow.Server.Models.Activity> UpdateActivityAsync(int id, UpdateActivityDto activityDto)
    {
        var activity = await _context.Activities
            .Include(a => a.Status)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (activity == null)
        {
            throw new KeyNotFoundException($"No se encontró la actividad con ID {id}");
        }

        // Buscar el estado por nombre
        var status = await _context.Statuses
            .FirstOrDefaultAsync(s => s.Name.ToString() == activityDto.Status);

        if (status == null)
        {
            throw new InvalidOperationException($"Estado no válido: {activityDto.Status}");
        }

        activity.Title = activityDto.Title;
        activity.Description = activityDto.Description;
        activity.CategoryId = activityDto.CategoryId;
        activity.StatusId = status.Id;

        await _context.SaveChangesAsync();
        return activity;
    }

    public async Task<bool> DeleteActivityAsync(int id)
    {
        var activity = await _context.Activities.FindAsync(id);
        if (activity == null)
        {
            return false;
        }

        _context.Activities.Remove(activity);
        await _context.SaveChangesAsync();
        return true;
    }
} 