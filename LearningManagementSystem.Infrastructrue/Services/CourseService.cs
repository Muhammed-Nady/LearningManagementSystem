using LearningManagementSystem.Core.DTOs.Common;
using LearningManagementSystem.Core.DTOs.Course;
using LearningManagementSystem.Core.Interfaces;
using LearningManagementSystem.Core.Interfaces.Services;
using LearningManagementSystem.Core.Models.Entities;
using LearningManagementSystem.Core.Models.Enums;

namespace LearningManagementSystem.Infrastructrue.Services
{
    public class CourseService : ICourseService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CourseService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResultDto<IEnumerable<CourseResponseDto>>> GetAllPublishedCoursesAsync()
        {
            var courses = await _unitOfWork.Courses.FindAsync(c => c.IsPublished);
            var courseDtos = new List<CourseResponseDto>();

            foreach (var course in courses)
            {
                courseDtos.Add(await MapToCourseResponseDto(course));
            }

            return ResultDto<IEnumerable<CourseResponseDto>>.SuccessResult(courseDtos);
        }

        public async Task<ResultDto<CourseResponseDto>> GetCourseByIdAsync(int courseId)
        {
            var course = await _unitOfWork.Courses.GetByIdAsync(courseId);
            if (course == null)
                return ResultDto<CourseResponseDto>.FailureResult("Course not found");

            var dto = await MapToCourseResponseDto(course);
            return ResultDto<CourseResponseDto>.SuccessResult(dto);
        }

        public async Task<ResultDto<IEnumerable<CourseResponseDto>>> GetCoursesByInstructorAsync(int instructorId)
        {
            var courses = await _unitOfWork.Courses.FindAsync(c => c.InstructorId == instructorId);
            var courseDtos = new List<CourseResponseDto>();

            foreach (var course in courses)
            {
                courseDtos.Add(await MapToCourseResponseDto(course));
            }

            return ResultDto<IEnumerable<CourseResponseDto>>.SuccessResult(courseDtos);
        }

        public async Task<ResultDto<IEnumerable<CourseResponseDto>>> GetCoursesByCategoryAsync(int categoryId)
        {
            var courses = await _unitOfWork.Courses.FindAsync(c => c.CategoryId == categoryId && c.IsPublished);
            var courseDtos = new List<CourseResponseDto>();

            foreach (var course in courses)
            {
                courseDtos.Add(await MapToCourseResponseDto(course));
            }

            return ResultDto<IEnumerable<CourseResponseDto>>.SuccessResult(courseDtos);
        }

        public async Task<ResultDto<CourseResponseDto>> CreateCourseAsync(CreateCourseDto dto, int instructorId)
        {
            // Validate instructor
            var instructor = await _unitOfWork.Users.GetByIdAsync(instructorId);
            if (instructor == null || instructor.Role != UserRole.Instructor)
                return ResultDto<CourseResponseDto>.FailureResult("Invalid instructor");

            // Validate category
            var category = await _unitOfWork.Categories.GetByIdAsync(dto.CategoryId);
            if (category == null)
                return ResultDto<CourseResponseDto>.FailureResult("Category not found");

            var course = new Course
            {
                Title = dto.Title,
                Description = dto.Description,
                InstructorId = instructorId,
                CategoryId = dto.CategoryId,
                ThumbnailUrl = dto.ThumbnailUrl,
                Duration = dto.Duration,
                Level = dto.Level,
                Price = dto.Price,
                IsPublished = false,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Courses.AddAsync(course);
            await _unitOfWork.SaveChangesAsync();

            var responseDto = await MapToCourseResponseDto(course);
            return ResultDto<CourseResponseDto>.SuccessResult(responseDto, "Course created successfully");
        }

        public async Task<ResultDto<CourseResponseDto>> UpdateCourseAsync(int courseId, UpdateCourseDto dto, int instructorId)
        {
            var course = await _unitOfWork.Courses.GetByIdAsync(courseId);
            if (course == null)
                return ResultDto<CourseResponseDto>.FailureResult("Course not found");

            if (course.InstructorId != instructorId)
                return ResultDto<CourseResponseDto>.FailureResult("Unauthorized");

            // Validate category
            var category = await _unitOfWork.Categories.GetByIdAsync(dto.CategoryId);
            if (category == null)
                return ResultDto<CourseResponseDto>.FailureResult("Category not found");

            course.Title = dto.Title;
            course.Description = dto.Description;
            course.CategoryId = dto.CategoryId;
            course.ThumbnailUrl = dto.ThumbnailUrl;
            course.Duration = dto.Duration;
            course.Level = dto.Level;
            course.Price = dto.Price;
            course.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Courses.Update(course);
            await _unitOfWork.SaveChangesAsync();

            var responseDto = await MapToCourseResponseDto(course);
            return ResultDto<CourseResponseDto>.SuccessResult(responseDto, "Course updated successfully");
        }

        public async Task<ResultDto<bool>> DeleteCourseAsync(int courseId, int instructorId)
        {
            var course = await _unitOfWork.Courses.GetByIdAsync(courseId);
            if (course == null)
                return ResultDto<bool>.FailureResult("Course not found");

            if (course.InstructorId != instructorId)
                return ResultDto<bool>.FailureResult("Unauthorized");

            _unitOfWork.Courses.Remove(course);
            await _unitOfWork.SaveChangesAsync();

            return ResultDto<bool>.SuccessResult(true, "Course deleted successfully");
        }

        public async Task<ResultDto<bool>> PublishCourseAsync(int courseId, int instructorId)
        {
            var course = await _unitOfWork.Courses.GetByIdAsync(courseId);
            if (course == null)
                return ResultDto<bool>.FailureResult("Course not found");

            if (course.InstructorId != instructorId)
                return ResultDto<bool>.FailureResult("Unauthorized");

            // Check if course has content
            var sections = await _unitOfWork.Sections.FindAsync(s => s.CourseId == courseId);
            if (!sections.Any())
                return ResultDto<bool>.FailureResult("Cannot publish course without content");

            course.IsPublished = true;
            course.UpdatedAt = DateTime.UtcNow;
            _unitOfWork.Courses.Update(course);
            await _unitOfWork.SaveChangesAsync();

            return ResultDto<bool>.SuccessResult(true, "Course published successfully");
        }

        public async Task<ResultDto<bool>> UnpublishCourseAsync(int courseId, int instructorId)
        {
            var course = await _unitOfWork.Courses.GetByIdAsync(courseId);
            if (course == null)
                return ResultDto<bool>.FailureResult("Course not found");

            if (course.InstructorId != instructorId)
                return ResultDto<bool>.FailureResult("Unauthorized");

            course.IsPublished = false;
            course.UpdatedAt = DateTime.UtcNow;
            _unitOfWork.Courses.Update(course);
            await _unitOfWork.SaveChangesAsync();

            return ResultDto<bool>.SuccessResult(true, "Course unpublished successfully");
        }

        private async Task<CourseResponseDto> MapToCourseResponseDto(Course course)
        {
            var instructor = await _unitOfWork.Users.GetByIdAsync(course.InstructorId);
            var category = await _unitOfWork.Categories.GetByIdAsync(course.CategoryId);
            var enrollmentCount = await _unitOfWork.Enrollments.CountAsync(e => e.CourseId == course.CourseId);

            var reviews = await _unitOfWork.Reviews.FindAsync(r => r.CourseId == course.CourseId);
            var averageRating = reviews.Any() ? reviews.Average(r => r.Rating) : 0;

            return new CourseResponseDto
            {
                CourseId = course.CourseId,
                Title = course.Title,
                Description = course.Description,
                InstructorId = course.InstructorId,
                InstructorName = instructor != null ? $"{instructor.FirstName} {instructor.LastName}" : "Unknown",
                CategoryId = course.CategoryId,
                CategoryName = category?.Name ?? "Unknown",
                ThumbnailUrl = course.ThumbnailUrl,
                IsPublished = course.IsPublished,
                Duration = course.Duration,
                Level = course.Level.ToString(),
                Price = course.Price,
                CreatedAt = course.CreatedAt,
                EnrollmentCount = enrollmentCount,
                AverageRating = averageRating
            };
        }
    }
}
