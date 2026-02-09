using LearningManagementSystem.Core.DTOs.Common;

namespace LearningManagementSystem.Core.Interfaces.Services
{
    public interface IProgressService
    {
        Task<ResultDto<bool>> MarkLessonCompleteAsync(int studentId, int lessonId);
        Task<ResultDto<decimal>> CalculateCourseProgressAsync(int studentId, int courseId);
        Task<ResultDto<int?>> GetLastAccessedLessonAsync(int studentId, int courseId);
    }
}
