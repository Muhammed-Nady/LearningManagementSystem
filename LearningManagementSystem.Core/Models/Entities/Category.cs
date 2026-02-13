
namespace LearningManagementSystem.Core.Models.Entities
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Course> Courses { get; set; } = new List<Course>();
    }
}
