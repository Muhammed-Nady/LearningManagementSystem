namespace LearningManagementSystem.Core.DTOs.Course
{
    public class CourseResponseDto
    {
        public int CourseId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int InstructorId { get; set; }
        public string InstructorName { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string? ThumbnailUrl { get; set; }
        public bool IsPublished { get; set; }
        public int? Duration { get; set; }
        public string Level { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; }
        public int EnrollmentCount { get; set; }
        public double AverageRating { get; set; }
    }
}

