using System.Collections.Generic;

namespace LearningManagementSystem.MVC.Models
{
    public class StudentDashboardViewModel
    {
        public int TotalEnrolledCourses { get; set; }
        public int CompletedCourses { get; set; }
        public int TotalHoursLearned { get; set; }
        public decimal AverageProgress { get; set; }
  public List<EnrolledCourseVm> ContinueLearning { get; set; } = new();
  public List<EnrolledCourseVm> RecentCourses { get; set; } = new();
    }

    public class MyCoursesViewModel
    {
     public List<EnrolledCourseVm> EnrolledCourses { get; set; } = new();
  public string Filter { get; set; } = "All"; // All, InProgress, Completed
    }

    public class EnrolledCourseVm
    {
        public int CourseId { get; set; }
        public string Title { get; set; } = string.Empty;
 public string InstructorName { get; set; } = string.Empty;
public string ThumbnailUrl { get; set; } = string.Empty;
        public decimal ProgressPercentage { get; set; }
    public string Status { get; set; } = string.Empty;
        public DateTime EnrolledAt { get; set; }
  public DateTime? CompletedAt { get; set; }
  public int? LastLessonId { get; set; }
    }

    public class LearnViewModel
{
        public int CourseId { get; set; }
      public string CourseTitle { get; set; } = string.Empty;
        public string InstructorName { get; set; } = string.Empty;
 public List<SectionVm> Sections { get; set; } = new();
  public int? CurrentLessonId { get; set; }
   public decimal ProgressPercentage { get; set; }
    }

    public class SectionVm
{
    public int SectionId { get; set; }
  public string Title { get; set; } = string.Empty;
   public int OrderIndex { get; set; }
        public List<LessonVm> Lessons { get; set; } = new();
    }

    public class LessonVm
    {
        public int LessonId { get; set; }
  public string Title { get; set; } = string.Empty;
 public string ContentType { get; set; } = string.Empty;
        public string? ContentUrl { get; set; }
        public int? Duration { get; set; }
        public int OrderIndex { get; set; }
   public bool IsCompleted { get; set; }
        public bool IsFree { get; set; }
    }
}
