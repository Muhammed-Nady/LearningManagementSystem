namespace LearningManagementSystem.MVC.Models
{
  public class AdminDashboardViewModel
    {
        public int TotalUsers { get; set; }
        public int ActiveUsers { get; set; }
        public int TotalCourses { get; set; }
        public int PublishedCourses { get; set; }
        public int TotalCategories { get; set; }
    }

public class CategoryDto
    {
        public int CategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
     public string? Description { get; set; }
   public DateTime CreatedAt { get; set; }
    }
}

