using LearningManagementSystem.Core.DTOs.Course;
using LearningManagementSystem.Core.DTOs.Common;

namespace LearningManagementSystem.Core.Interfaces.Services
{
    public interface ICourseService
    {
        Task<ResultDto<IEnumerable<CourseResponseDto>>> GetAllPublishedCoursesAsync();
        Task<ResultDto<CourseResponseDto>> GetCourseByIdAsync(int courseId);
        Task<ResultDto<IEnumerable<CourseResponseDto>>> GetCoursesByInstructorAsync(int instructorId);
        Task<ResultDto<IEnumerable<CourseResponseDto>>> GetCoursesByCategoryAsync(int categoryId);

        Task<ResultDto<CourseResponseDto>> CreateCourseAsync(CreateCourseDto dto, int instructorId);
        Task<ResultDto<CourseResponseDto>> UpdateCourseAsync(int courseId, UpdateCourseDto dto, int instructorId);
        Task<ResultDto<bool>> DeleteCourseAsync(int courseId, int instructorId);

        Task<ResultDto<bool>> PublishCourseAsync(int courseId, int instructorId);
        Task<ResultDto<bool>> UnpublishCourseAsync(int courseId, int instructorId);
    }
}
