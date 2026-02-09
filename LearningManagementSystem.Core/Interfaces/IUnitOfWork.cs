using LearningManagementSystem.Core.Models.Entities;

namespace LearningManagementSystem.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        // Repositories
        IRepository<User> Users { get; }
        IRepository<Category> Categories { get; }
        IRepository<Course> Courses { get; }
        IRepository<Section> Sections { get; }
        IRepository<Lesson> Lessons { get; }
        IRepository<Enrollment> Enrollments { get; }
        IRepository<Progress> ProgressRecords { get; }
        IRepository<Review> Reviews { get; }

        // Save changes
        Task<int> SaveChangesAsync();
        int SaveChanges();
    }
}
