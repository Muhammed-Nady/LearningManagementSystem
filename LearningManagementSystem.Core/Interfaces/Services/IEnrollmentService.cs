using LearningManagementSystem.Core.DTOs.Common;

namespace LearningManagementSystem.Core.Interfaces.Services
{
    public interface IEnrollmentService
    {
        Task<ResultDto<bool>> EnrollStudentAsync(int studentId, int courseId);
        Task<ResultDto<bool>> UnenrollStudentAsync(int studentId, int courseId);
        Task<ResultDto<bool>> IsStudentEnrolledAsync(int studentId, int courseId);
        Task<ResultDto<IEnumerable<int>>> GetStudentCourseIdsAsync(int studentId);
    }
}

