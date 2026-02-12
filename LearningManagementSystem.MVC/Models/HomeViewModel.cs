using System.Collections.Generic;

namespace LearningManagementSystem.MVC.Models
{
    public class HomeViewModel
    {
        public List<CourseCardVm> FeaturedCourses { get; set; } = new();
        public List<CategoryVm> Categories { get; set; } = new();
    }

    public class CourseCardVm
    {
        public int CourseId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string InstructorName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ThumbnailUrl { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public bool IsPublished { get; set; }
        public string Level { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public double AverageRating { get; set; }
        public int EnrollmentCount { get; set; }
    }

    public class CategoryVm
    {
        public int CategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class CoursesIndexViewModel
    {
        public List<CourseCardVm> Courses { get; set; } = new();
        public List<CategoryVm> Categories { get; set; } = new();
        public int? SelectedCategory { get; set; }
        public string? SelectedLevel { get; set; }
        public string? SearchTerm { get; set; }
    }

    public class CourseDetailsViewModel
    {
        public int CourseId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int InstructorId { get; set; }
        public string InstructorName { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public string ThumbnailUrl { get; set; } = string.Empty;
        public bool IsPublished { get; set; }
        public int? Duration { get; set; }
        public string Level { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int EnrollmentCount { get; set; }
        public double AverageRating { get; set; }
    }
}
