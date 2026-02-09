// file: LearningManagementSystem.MVC/Models/Entities/Course.cs
using LearningManagementSystem.Core.Models.Enums;

namespace LearningManagementSystem.Core.Models.Entities
{
    public class Course
    {
        public int CourseId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int InstructorId { get; set; }
        public int CategoryId { get; set; }
        public string? ThumbnailUrl { get; set; }
        public bool IsPublished { get; set; } = false;
        public int? Duration { get; set; } // in hours
        public CourseLevel Level { get; set; } = CourseLevel.Beginner;
        public decimal Price { get; set; } = 0;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Navigation Properties
        public User Instructor { get; set; } = null!;
        public Category Category { get; set; } = null!;
        public ICollection<Section> Sections { get; set; } = new List<Section>();
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}