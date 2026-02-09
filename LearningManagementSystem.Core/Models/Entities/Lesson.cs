// file: LearningManagementSystem.Core/Models/Entities/Lesson.cs
using LearningManagementSystem.Core.Models.Enums;

namespace LearningManagementSystem.Core.Models.Entities
{
    public class Lesson
    {
        public int LessonId { get; set; }
        public int SectionId { get; set; }
        public string Title { get; set; } = string.Empty;
        public ContentType ContentType { get; set; }
        public string? ContentUrl { get; set; }
        public string? TextContent { get; set; }
        public int? Duration { get; set; } // in minutes
        public int OrderIndex { get; set; }
        public bool IsFree { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public Section Section { get; set; } = null!;
        public ICollection<Progress> ProgressRecords { get; set; } = new List<Progress>();
    }
}