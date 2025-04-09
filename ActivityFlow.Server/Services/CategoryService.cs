using ActivityFlow.Server.Data;
using ActivityFlow.Server.Models;
using ActivityFlow.Shared.Models;
using Microsoft.EntityFrameworkCore;

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

    public async Task<List<CategoryDto>> GetAllCategoriesAsync()
    {
        return await _context.Categories
            .Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                Color = c.Color,
                IsActive = c.IsActive
            })
            .ToListAsync();
    }

    public async Task<CategoryDto?> GetCategoryByIdAsync(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null) return null;

        return new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            Color = category.Color,
            IsActive = category.IsActive
        };
    }

    public async Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto categoryDto)
    {
        var category = new ActivityFlow.Server.Models.Category
        {
            Name = categoryDto.Name,
            Description = categoryDto.Description,
            Color = categoryDto.Color,
            IsActive = true
        };

        _context.Categories.Add(category);
        await _context.SaveChangesAsync();

        return new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            Color = category.Color,
            IsActive = category.IsActive
        };
    }

    public async Task<CategoryDto?> UpdateCategoryAsync(int id, UpdateCategoryDto categoryDto)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null) return null;

        if (categoryDto.Name != null)
            category.Name = categoryDto.Name;
        
        if (categoryDto.Description != null)
            category.Description = categoryDto.Description;
        
        if (categoryDto.Color != null)
            category.Color = categoryDto.Color;
        
        if (categoryDto.IsActive.HasValue)
            category.IsActive = categoryDto.IsActive.Value;

        category.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            Color = category.Color,
            IsActive = category.IsActive
        };
    }

    public async Task<bool> DeleteCategoryAsync(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null) return false;

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();

        return true;
    }
} 