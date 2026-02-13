using LearningManagementSystem.Core.DTOs.Common;
using LearningManagementSystem.Core.Models.Entities;

namespace LearningManagementSystem.Core.Interfaces.Services
{
    public interface IReviewService
    {
        Task<ResultDto<Review>> SubmitReviewAsync(int studentId, int courseId, int rating, string? comment);
        Task<ResultDto<IEnumerable<Review>>> GetCourseReviewsAsync(int courseId);
        Task<ResultDto<double>> GetCourseAverageRatingAsync(int courseId);
        Task<ResultDto<bool>> DeleteReviewAsync(int reviewId, int studentId);
    }
}

