
using LearningManagementSystem.Core.Models.Enums;

namespace LearningManagementSystem.Core.Models.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public UserRole Role { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public ICollection<Course> CoursesAsInstructor { get; set; } = new List<Course>();
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
        public ICollection<Progress> ProgressRecords { get; set; } = new List<Progress>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
