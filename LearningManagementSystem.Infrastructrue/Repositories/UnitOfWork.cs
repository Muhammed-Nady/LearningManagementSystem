using LearningManagementSystem.Core.Interfaces;
using LearningManagementSystem.Core.Models.Entities;
using LearningManagementSystem.Infrastructrue.Data;

namespace LearningManagementSystem.Infrastructrue.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        private IRepository<User>? _users;
        private IRepository<Category>? _categories;
        private IRepository<Course>? _courses;
        private IRepository<Section>? _sections;
        private IRepository<Lesson>? _lessons;
        private IRepository<Enrollment>? _enrollments;
        private IRepository<Progress>? _progressRecords;
        private IRepository<Review>? _reviews;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public IRepository<User> Users => _users ??= new Repository<User>(_context);
        public IRepository<Category> Categories => _categories ??= new Repository<Category>(_context);
        public IRepository<Course> Courses => _courses ??= new Repository<Course>(_context);
        public IRepository<Section> Sections => _sections ??= new Repository<Section>(_context);
        public IRepository<Lesson> Lessons => _lessons ??= new Repository<Lesson>(_context);
        public IRepository<Enrollment> Enrollments => _enrollments ??= new Repository<Enrollment>(_context);
        public IRepository<Progress> ProgressRecords => _progressRecords ??= new Repository<Progress>(_context);
        public IRepository<Review> Reviews => _reviews ??= new Repository<Review>(_context);

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}

