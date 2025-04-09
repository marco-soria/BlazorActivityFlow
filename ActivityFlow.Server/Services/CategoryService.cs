using ActivityFlow.Server.Models;
using Microsoft.EntityFrameworkCore;
using ActivityFlow.Server.Data;

namespace ActivityFlow.Server.Services;

public class CategoryService : ICategoryService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<CategoryService> _logger;

    public CategoryService(ApplicationDbContext context, ILogger<CategoryService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<List<Category>> GetAllCategoriesAsync()
    {
        try
        {
            return await _context.Categories.ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener todas las categorías");
            throw;
        }
    }

    public async Task<Category?> GetCategoryByIdAsync(int id)
    {
        try
        {
            return await _context.Categories.FindAsync(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener categoría por ID: {CategoryId}", id);
            throw;
        }
    }

    public async Task<Category> CreateCategoryAsync(CreateCategoryDto categoryDto)
    {
        try
        {
            var category = new Category
            {
                Name = categoryDto.Name,
                Description = categoryDto.Description
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return category;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear categoría: {CategoryName}", categoryDto.Name);
            throw;
        }
    }

    public async Task<Category> UpdateCategoryAsync(int id, UpdateCategoryDto categoryDto)
    {
        try
        {
            var existingCategory = await _context.Categories.FindAsync(id);
            if (existingCategory == null)
            {
                throw new KeyNotFoundException($"Categoría con ID {id} no encontrada");
            }

            existingCategory.Name = categoryDto.Name;
            existingCategory.Description = categoryDto.Description;

            await _context.SaveChangesAsync();
            return existingCategory;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar categoría: {CategoryId}", id);
            throw;
        }
    }

    public async Task<bool> DeleteCategoryAsync(int id)
    {
        try
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return false;
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar categoría: {CategoryId}", id);
            throw;
        }
    }
} 