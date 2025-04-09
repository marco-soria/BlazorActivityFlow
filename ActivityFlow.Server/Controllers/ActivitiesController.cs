using ActivityFlow.Server.Models;
using ActivityFlow.Server.Services;
using ActivityFlow.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ActivityFlow.Server.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ActivitiesController : ControllerBase
{
    private readonly IActivityService _activityService;
    private readonly ICategoryService _categoryService;
    private readonly ILogger<ActivitiesController> _logger;

    public ActivitiesController(
        IActivityService activityService,
        ICategoryService categoryService,
        ILogger<ActivitiesController> logger)
    {
        _activityService = activityService;
        _categoryService = categoryService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<List<ActivityFlow.Server.Models.Activity>>> GetAllActivities()
    {
        try
        {
            var activities = await _activityService.GetAllActivitiesAsync();
            return Ok(activities);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener todas las actividades");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ActivityFlow.Server.Models.Activity>> GetActivityById(int id)
    {
        try
        {
            var activity = await _activityService.GetActivityByIdAsync(id);
            if (activity == null)
            {
                return NotFound();
            }
            return Ok(activity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener la actividad con ID {Id}", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpGet("user/{userId}")]
    public async Task<ActionResult<List<ActivityFlow.Server.Models.Activity>>> GetActivitiesByUserId(string userId)
    {
        try
        {
            var activities = await _activityService.GetActivitiesByUserIdAsync(userId);
            return Ok(activities);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener actividades del usuario {UserId}", userId);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpGet("category/{categoryId}")]
    public async Task<ActionResult<List<ActivityFlow.Server.Models.Activity>>> GetActivitiesByCategoryId(int categoryId)
    {
        try
        {
            var activities = await _activityService.GetActivitiesByCategoryIdAsync(categoryId);
            return Ok(activities);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener actividades de la categoría {CategoryId}", categoryId);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpGet("categories")]
    public async Task<ActionResult<List<ActivityFlow.Server.Models.Category>>> GetCategories()
    {
        try
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return Ok(categories);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener las categorías");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpPost]
    public async Task<ActionResult<ActivityFlow.Server.Models.Activity>> CreateActivity(CreateActivityDto activityDto)
    {
        try
        {
            var activity = await _activityService.CreateActivityAsync(activityDto);
            return CreatedAtAction(nameof(GetActivityById), new { id = activity.Id }, activity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear la actividad");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ActivityFlow.Server.Models.Activity>> UpdateActivity(int id, UpdateActivityDto activityDto)
    {
        try
        {
            var activity = await _activityService.UpdateActivityAsync(id, activityDto);
            return Ok(activity);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar la actividad con ID {Id}", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteActivity(int id)
    {
        try
        {
            var result = await _activityService.DeleteActivityAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar la actividad con ID {Id}", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }
} 