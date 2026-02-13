using System.ComponentModel.DataAnnotations;

namespace LearningManagementSystem.MVC.Models
{
    public class InstructorDashboardViewModel
    {
        public List<CourseResponseDto> Courses { get; set; } = new();
    public int TotalCourses => Courses.Count;
        public int PublishedCourses => Courses.Count(c => c.IsPublished);
        public int TotalStudents => Courses.Sum(c => c.EnrollmentCount);
        public double AverageRating => Courses.Any() ? Courses.Average(c => c.AverageRating) : 0;
    }

    public class CreateCourseViewModel
    {
        [Required(ErrorMessage = "Title is required")]
        [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
        public string Title { get; set; } = string.Empty;

     [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; } = string.Empty;

      [Required(ErrorMessage = "Category is required")]
    [Display(Name = "Category")]
        public int CategoryId { get; set; }

        [Display(Name = "Thumbnail URL")]
        [Url(ErrorMessage = "Invalid URL format")]
        public string? ThumbnailUrl { get; set; }

   [Display(Name = "Duration (hours)")]
        [Range(1, 1000, ErrorMessage = "Duration must be between 1 and 1000 hours")]
   public int? Duration { get; set; }

        [Required(ErrorMessage = "Level is required")]
   public string Level { get; set; } = "Beginner";

        [Required(ErrorMessage = "Price is required")]
        [Range(0, 10000, ErrorMessage = "Price must be between 0 and 10000")]
     [DataType(DataType.Currency)]
     public decimal Price { get; set; } = 0;
    }

    public class EditCourseViewModel
    {
        public int CourseId { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Category is required")]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

      [Display(Name = "Thumbnail URL")]
        [Url(ErrorMessage = "Invalid URL format")]
        public string? ThumbnailUrl { get; set; }

     [Display(Name = "Duration (hours)")]
        [Range(1, 1000, ErrorMessage = "Duration must be between 1 and 1000 hours")]
 public int? Duration { get; set; }

        [Required(ErrorMessage = "Level is required")]
        public string Level { get; set; } = "Beginner";

        [Required(ErrorMessage = "Price is required")]
        [Range(0, 10000, ErrorMessage = "Price must be between 0 and 10000")]
   [DataType(DataType.Currency)]
        public decimal Price { get; set; }

   public bool IsPublished { get; set; }
    }

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
        public int EnrollmentCount { get; set; }
        public double AverageRating { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

