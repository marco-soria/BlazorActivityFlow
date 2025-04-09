using ActivityFlow.Shared.Models;

namespace ActivityFlow.Server.Services;

public interface ICategoryService
{
    Task<List<CategoryDto>> GetAllCategoriesAsync();
    Task<CategoryDto?> GetCategoryByIdAsync(int id);
    Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto categoryDto);
    Task<CategoryDto?> UpdateCategoryAsync(int id, UpdateCategoryDto categoryDto);
    Task<bool> DeleteCategoryAsync(int id);
} 