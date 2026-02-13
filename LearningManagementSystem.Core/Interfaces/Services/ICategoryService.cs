using LearningManagementSystem.Core.DTOs.Common;
using LearningManagementSystem.Core.Models.Entities;

namespace LearningManagementSystem.Core.Interfaces.Services
{
    public interface ICategoryService
    {
        Task<ResultDto<IEnumerable<Category>>> GetAllCategoriesAsync();
        Task<ResultDto<Category>> GetCategoryByIdAsync(int categoryId);
        Task<ResultDto<Category>> CreateCategoryAsync(string name, string? description);
        Task<ResultDto<bool>> DeleteCategoryAsync(int categoryId);
    }
}

