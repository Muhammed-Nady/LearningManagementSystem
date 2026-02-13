using LearningManagementSystem.Core.Models.Enums;

namespace LearningManagementSystem.Core.DTOs.Course
{
    public class UpdateCourseDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public string? ThumbnailUrl { get; set; }
        public int? Duration { get; set; }
        public CourseLevel Level { get; set; }
        public decimal Price { get; set; }
    }
}

