using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ActivityFlow.Server.Services;
using ActivityFlow.Server.Models;

namespace ActivityFlow.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
//[Authorize]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;
    private readonly ILogger<CategoriesController> _logger;

    public CategoriesController(ICategoryService categoryService, ILogger<CategoriesController> logger)
    {
        _categoryService = categoryService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<List<Category>>> GetAllCategories()
    {
        try
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return Ok(categories);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener todas las categorías");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Category>> GetCategoryById(int id)
    {
        try
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category is null)
            {
                return NotFound("Categoría no encontrada");
            }
            return Ok(category);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener categoría por ID: {CategoryId}", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpPost]
    //[Authorize(Roles = "Admin")]
    public async Task<ActionResult<Category>> CreateCategory([FromBody] CreateCategoryDto categoryDto)
    {
        try
        {
            var createdCategory = await _categoryService.CreateCategoryAsync(categoryDto);
            return CreatedAtAction(nameof(GetCategoryById), new { id = createdCategory.Id }, createdCategory);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear categoría: {CategoryName}", categoryDto.Name);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpPut("{id}")]
    //[Authorize(Roles = "Admin")]
    public async Task<ActionResult<Category>> UpdateCategory(int id, [FromBody] UpdateCategoryDto categoryDto)
    {
        try
        {
            var updatedCategory = await _categoryService.UpdateCategoryAsync(id, categoryDto);
            return Ok(updatedCategory);
        }
        catch (KeyNotFoundException)
        {
            return NotFound("Categoría no encontrada");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar categoría: {CategoryId}", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpDelete("{id}")]
    //[Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        try
        {
            var result = await _categoryService.DeleteCategoryAsync(id);
            if (!result)
            {
                return NotFound("Categoría no encontrada");
            }
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar categoría: {CategoryId}", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }
} 