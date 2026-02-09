// file: LearningManagementSystem.MVC/Models/Entities/Enrollment.cs
using LearningManagementSystem.Core.Models.Enums;

namespace LearningManagementSystem.Core.Models.Entities
{
    public class Enrollment
    {
   public int EnrollmentId { get; set; }
        public int StudentId { get; set; }
        public int CourseId { get; set; }
     public DateTime EnrolledAt { get; set; } = DateTime.UtcNow;
        public DateTime? CompletedAt { get; set; }
   public EnrollmentStatus Status { get; set; } = EnrollmentStatus.Active;
     public decimal ProgressPercentage { get; set; } = 0;

        // Navigation Properties
   public User Student { get; set; } = null!;
        public Course Course { get; set; } = null!;
    }
}