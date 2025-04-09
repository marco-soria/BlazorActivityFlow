using ActivityFlow.Server.Models;

namespace ActivityFlow.Server.Services;

public interface ICategoryService
{
    Task<List<Category>> GetAllCategoriesAsync();
    Task<Category?> GetCategoryByIdAsync(int id);
    Task<Category> CreateCategoryAsync(CreateCategoryDto categoryDto);
    Task<Category> UpdateCategoryAsync(int id, UpdateCategoryDto categoryDto);
    Task<bool> DeleteCategoryAsync(int id);
} 