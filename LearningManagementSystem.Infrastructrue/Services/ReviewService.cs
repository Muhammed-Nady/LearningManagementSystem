using LearningManagementSystem.Core.DTOs.Common;
using LearningManagementSystem.Core.Interfaces;
using LearningManagementSystem.Core.Interfaces.Services;
using LearningManagementSystem.Core.Models.Entities;
using LearningManagementSystem.Core.Models.Enums;

namespace LearningManagementSystem.Infrastructrue.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReviewService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResultDto<Review>> SubmitReviewAsync(int studentId, int courseId, int rating, string? comment)
        {

            var student = await _unitOfWork.Users.GetByIdAsync(studentId);
            if (student == null || student.Role != UserRole.Student)
                return ResultDto<Review>.FailureResult("Invalid student");

            var course = await _unitOfWork.Courses.GetByIdAsync(courseId);
            if (course == null)
                return ResultDto<Review>.FailureResult("Course not found");

            var isEnrolled = await _unitOfWork.Enrollments
             .AnyAsync(e => e.StudentId == studentId && e.CourseId == courseId);

            if (!isEnrolled)
                return ResultDto<Review>.FailureResult("Must be enrolled to review");

            var existingReview = await _unitOfWork.Reviews
                  .FirstOrDefaultAsync(r => r.StudentId == studentId && r.CourseId == courseId);

            if (existingReview != null)
                return ResultDto<Review>.FailureResult("Review already submitted");

            if (rating < 1 || rating > 5)
                return ResultDto<Review>.FailureResult("Rating must be between 1 and 5");

            var review = new Review
            {
                CourseId = courseId,
                StudentId = studentId,
                Rating = rating,
                Comment = comment,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Reviews.AddAsync(review);
            await _unitOfWork.SaveChangesAsync();

            return ResultDto<Review>.SuccessResult(review, "Review submitted successfully");
        }

        public async Task<ResultDto<IEnumerable<Review>>> GetCourseReviewsAsync(int courseId)
        {
            var reviews = await _unitOfWork.Reviews.FindAsync(r => r.CourseId == courseId);
            return ResultDto<IEnumerable<Review>>.SuccessResult(reviews);
        }

        public async Task<ResultDto<double>> GetCourseAverageRatingAsync(int courseId)
        {
            var reviews = await _unitOfWork.Reviews.FindAsync(r => r.CourseId == courseId);
            var averageRating = reviews.Any() ? reviews.Average(r => r.Rating) : 0;
            return ResultDto<double>.SuccessResult(averageRating);
        }

        public async Task<ResultDto<bool>> DeleteReviewAsync(int reviewId, int studentId)
        {
            var review = await _unitOfWork.Reviews.GetByIdAsync(reviewId);
            if (review == null)
                return ResultDto<bool>.FailureResult("Review not found");

            if (review.StudentId != studentId)
                return ResultDto<bool>.FailureResult("Unauthorized");

            _unitOfWork.Reviews.Remove(review);
            await _unitOfWork.SaveChangesAsync();

            return ResultDto<bool>.SuccessResult(true, "Review deleted successfully");
        }
    }
}

