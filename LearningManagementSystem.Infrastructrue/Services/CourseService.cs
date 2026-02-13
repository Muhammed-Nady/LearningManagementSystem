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
   // Use eager loading to prevent N+1 queries
     var courses = await _unitOfWork.Courses.FindWithIncludesAsync(
   c => c.IsPublished,
  c => c.Instructor,
 c => c.Category,
   c => c.Reviews,
  c => c.Enrollments
);

          var courseDtos = courses.Select(course => new CourseResponseDto
     {
       CourseId = course.CourseId,
      Title = course.Title,
   Description = course.Description,
  InstructorId = course.InstructorId,
           // Add null-safety check
InstructorName = course.Instructor != null 
        ? $"{course.Instructor.FirstName} {course.Instructor.LastName}" 
    : "Unknown Instructor",
CategoryId = course.CategoryId,
           // Add null-safety check
         CategoryName = course.Category?.Name ?? "Uncategorized",
     ThumbnailUrl = course.ThumbnailUrl,
     IsPublished = course.IsPublished,
Duration = course.Duration,
  Level = course.Level.ToString(),
   Price = course.Price,
     CreatedAt = course.CreatedAt,
     // Add null-safety check
  EnrollmentCount = course.Enrollments?.Count ?? 0,
        // Add null-safety check
     AverageRating = course.Reviews != null && course.Reviews.Any() 
        ? course.Reviews.Average(r => r.Rating) 
                    : 0
    }).ToList();

   return ResultDto<IEnumerable<CourseResponseDto>>.SuccessResult(courseDtos);
     }

        public async Task<ResultDto<CourseResponseDto>> GetCourseByIdAsync(int courseId)
        {
   var course = await _unitOfWork.Courses.GetByIdWithIncludesAsync(
    courseId,
        c => c.Instructor,
   c => c.Category,
  c => c.Reviews,
       c => c.Enrollments,
 c => c.Sections
     );

          if (course == null)
       return ResultDto<CourseResponseDto>.FailureResult("Course not found");

         // Handle null navigation properties safely
            var instructorName = course.Instructor != null 
    ? $"{course.Instructor.FirstName} {course.Instructor.LastName}" 
       : "Unknown Instructor";

      var categoryName = course.Category?.Name ?? "Uncategorized";

     var dto = new CourseResponseDto
         {
   CourseId = course.CourseId,
    Title = course.Title,
   Description = course.Description,
    InstructorId = course.InstructorId,
      InstructorName = instructorName,
         CategoryId = course.CategoryId,
          CategoryName = categoryName,
     ThumbnailUrl = course.ThumbnailUrl,
          IsPublished = course.IsPublished,
         Duration = course.Duration,
         Level = course.Level.ToString(),
         Price = course.Price,
 CreatedAt = course.CreatedAt,
     EnrollmentCount = course.Enrollments?.Count ?? 0,
     AverageRating = course.Reviews != null && course.Reviews.Any() 
       ? course.Reviews.Average(r => r.Rating) 
    : 0
       };

 return ResultDto<CourseResponseDto>.SuccessResult(dto);
  }

  public async Task<ResultDto<IEnumerable<CourseResponseDto>>> GetCoursesByInstructorAsync(int instructorId)
        {
  var courses = await _unitOfWork.Courses.FindWithIncludesAsync(
   c => c.InstructorId == instructorId,
   c => c.Instructor,
 c => c.Category,
   c => c.Reviews,
    c => c.Enrollments
 );

      var courseDtos = courses.Select(course => new CourseResponseDto
{
     CourseId = course.CourseId,
       Title = course.Title,
Description = course.Description,
     InstructorId = course.InstructorId,
        // Add null-safety check
InstructorName = course.Instructor != null 
    ? $"{course.Instructor.FirstName} {course.Instructor.LastName}" 
   : "Unknown Instructor",
   CategoryId = course.CategoryId,
           // Add null-safety check
CategoryName = course.Category?.Name ?? "Uncategorized",
   ThumbnailUrl = course.ThumbnailUrl,
  IsPublished = course.IsPublished,
        Duration = course.Duration,
     Level = course.Level.ToString(),
     Price = course.Price,
CreatedAt = course.CreatedAt,
  // Add null-safety checks
EnrollmentCount = course.Enrollments?.Count ?? 0,
   AverageRating = course.Reviews != null && course.Reviews.Any() 
   ? course.Reviews.Average(r => r.Rating) 
     : 0
 }).ToList();

   return ResultDto<IEnumerable<CourseResponseDto>>.SuccessResult(courseDtos);
        }

        public async Task<ResultDto<IEnumerable<CourseResponseDto>>> GetCoursesByCategoryAsync(int categoryId)
  {
       var courses = await _unitOfWork.Courses.FindWithIncludesAsync(
       c => c.CategoryId == categoryId && c.IsPublished,
c => c.Instructor,
         c => c.Category,
      c => c.Reviews,
      c => c.Enrollments
  );

   var courseDtos = courses.Select(course => new CourseResponseDto
     {
   CourseId = course.CourseId,
       Title = course.Title,
      Description = course.Description,
 InstructorId = course.InstructorId,
        // Add null-safety check
InstructorName = course.Instructor != null 
        ? $"{course.Instructor.FirstName} {course.Instructor.LastName}" 
    : "Unknown Instructor",
CategoryId = course.CategoryId,
       // Add null-safety check
CategoryName = course.Category?.Name ?? "Uncategorized",
  ThumbnailUrl = course.ThumbnailUrl,
   IsPublished = course.IsPublished,
  Duration = course.Duration,
 Level = course.Level.ToString(),
 Price = course.Price,
  CreatedAt = course.CreatedAt,
       // Add null-safety checks
    EnrollmentCount = course.Enrollments?.Count ?? 0,
     AverageRating = course.Reviews != null && course.Reviews.Any() 
    ? course.Reviews.Average(r => r.Rating) 
  : 0
  }).ToList();

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

            // TODO: Re-enable this check once section/lesson management is implemented
            // Check if course has content
          // var sections = await _unitOfWork.Sections.FindAsync(s => s.CourseId == courseId);
            // if (!sections.Any())
            //return ResultDto<bool>.FailureResult("Cannot publish course without content");

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
