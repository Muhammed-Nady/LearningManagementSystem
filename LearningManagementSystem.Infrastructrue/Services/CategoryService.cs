using LearningManagementSystem.Core.DTOs.Common;
using LearningManagementSystem.Core.Interfaces;
using LearningManagementSystem.Core.Interfaces.Services;
using LearningManagementSystem.Core.Models.Entities;

namespace LearningManagementSystem.Infrastructrue.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResultDto<IEnumerable<Category>>> GetAllCategoriesAsync()
        {
            var categories = await _unitOfWork.Categories.GetAllAsync();
            return ResultDto<IEnumerable<Category>>.SuccessResult(categories);
        }

        public async Task<ResultDto<Category>> GetCategoryByIdAsync(int categoryId)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(categoryId);
            if (category == null)
                return ResultDto<Category>.FailureResult("Category not found");

            return ResultDto<Category>.SuccessResult(category);
        }

        public async Task<ResultDto<Category>> CreateCategoryAsync(string name, string? description)
        {
            var category = new Category
            {
                Name = name,
                Description = description,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Categories.AddAsync(category);
            await _unitOfWork.SaveChangesAsync();

            return ResultDto<Category>.SuccessResult(category, "Category created successfully");
        }

        public async Task<ResultDto<bool>> DeleteCategoryAsync(int categoryId)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(categoryId);
            if (category == null)
                return ResultDto<bool>.FailureResult("Category not found");

            var hasCourses = await _unitOfWork.Courses.AnyAsync(c => c.CategoryId == categoryId);
            if (hasCourses)
                return ResultDto<bool>.FailureResult("Cannot delete category with existing courses");

            _unitOfWork.Categories.Remove(category);
            await _unitOfWork.SaveChangesAsync();

            return ResultDto<bool>.SuccessResult(true, "Category deleted successfully");
        }
    }
}

