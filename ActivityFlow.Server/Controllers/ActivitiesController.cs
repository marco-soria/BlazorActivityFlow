using ActivityFlow.Server.Models;
using ActivityFlow.Server.Services;
using ActivityFlow.Shared.Models;
using ActivityFlow.Server.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

using ServerActivity = ActivityFlow.Server.Models.Activity;
using SharedActivity = ActivityFlow.Shared.Models.Activity;
using ServerCategory = ActivityFlow.Server.Models.Category;
using SharedCategory = ActivityFlow.Shared.Models.Category;

namespace ActivityFlow.Server.Controllers;

//[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ActivitiesController : ControllerBase
{
    private readonly IActivityService _activityService;
    private readonly ILogger<ActivitiesController> _logger;
    private readonly ApplicationDbContext _context;

    public ActivitiesController(IActivityService activityService, ILogger<ActivitiesController> logger, ApplicationDbContext context)
    {
        _activityService = activityService;
        _logger = logger;
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<SharedActivity>>> GetAllActivities()
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
    public async Task<ActionResult<SharedActivity>> GetActivityById(int id)
    {
        try
        {
            var activity = await _activityService.GetActivityByIdAsync(id);
            if (activity == null)
                return NotFound();

            return Ok(activity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener la actividad con ID {Id}", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpGet("user/{userId}")]
    public async Task<ActionResult<List<SharedActivity>>> GetActivitiesByUserId(string userId)
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
    public async Task<ActionResult<List<SharedActivity>>> GetActivitiesByCategoryId(int categoryId)
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

    [HttpPost]
    public async Task<ActionResult<SharedActivity>> CreateActivity(CreateActivityDto activityDto)
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

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
    public async Task<IActionResult> UpdateActivity(int id, UpdateActivityDto activityDto)
    {
        try
        {
            var activity = await _activityService.UpdateActivityAsync(id, activityDto);
            if (activity == null)
                return NotFound();

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar la actividad con ID {Id}", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteActivity(int id)
    {
        try
        {
            var result = await _activityService.DeleteActivityAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar la actividad con ID {Id}", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpGet("categories")]
    public async Task<ActionResult<List<ServerCategory>>> GetCategories()
    {
        try
        {
            var categories = await _context.Categories.ToListAsync();
            return Ok(categories);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener las categorías");
            return StatusCode(500, "Error interno del servidor al obtener las categorías");
        }
    }
} 