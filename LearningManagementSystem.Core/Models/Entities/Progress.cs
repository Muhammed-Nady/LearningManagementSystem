// file: LearningManagementSystem.Core/Models/Entities/Progress.cs
namespace LearningManagementSystem.Core.Models.Entities
{
    public class Progress
    {
        public int ProgressId { get; set; }
        public int StudentId { get; set; }
        public int LessonId { get; set; }
        public bool IsCompleted { get; set; } = false;
        public DateTime LastAccessedAt { get; set; } = DateTime.UtcNow;
        public DateTime? CompletedAt { get; set; }

        // Navigation Properties
        public User Student { get; set; } = null!;
        public Lesson Lesson { get; set; } = null!;
    }
}