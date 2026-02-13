using LearningManagementSystem.Core.Models.Entities;
using LearningManagementSystem.Core.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace LearningManagementSystem.Infrastructrue.Data
{
    public class ApplicationDbContext : DbContext
    {
        // DbSets for all entities
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Progress> ProgressRecords { get; set; }
        public DbSet<Review> Reviews { get; set; }

        // Configuration passed via constructor
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Entity configurations will be added here
            // For now, we'll add basic configurations

            // User configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserId);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.Email).IsRequired().HasMaxLength(256);
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
            });

            // Category configuration
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.CategoryId);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            });

            // Course configuration
            modelBuilder.Entity<Course>(entity =>
            {
                entity.HasKey(e => e.CourseId);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Description).IsRequired();
                entity.Property(e => e.Price).HasColumnType("decimal(18,2)");

                // Relationships
                entity.HasOne(e => e.Instructor)
                    .WithMany(u => u.CoursesAsInstructor)
                    .HasForeignKey(e => e.InstructorId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Category)
                    .WithMany(c => c.Courses)
                    .HasForeignKey(e => e.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Section configuration
            modelBuilder.Entity<Section>(entity =>
            {
                entity.HasKey(e => e.SectionId);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);

                entity.HasOne(e => e.Course)
                    .WithMany(c => c.Sections)
                    .HasForeignKey(e => e.CourseId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Lesson configuration
            modelBuilder.Entity<Lesson>(entity =>
            {
                entity.HasKey(e => e.LessonId);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);

                entity.HasOne(e => e.Section)
                    .WithMany(s => s.Lessons)
                    .HasForeignKey(e => e.SectionId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Enrollment configuration
            modelBuilder.Entity<Enrollment>(entity =>
            {
                entity.HasKey(e => e.EnrollmentId);
                entity.HasIndex(e => new { e.StudentId, e.CourseId }).IsUnique();
                entity.Property(e => e.ProgressPercentage).HasColumnType("decimal(5,2)");

                entity.HasOne(e => e.Student)
                    .WithMany(u => u.Enrollments)
                    .HasForeignKey(e => e.StudentId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Course)
                    .WithMany(c => c.Enrollments)
                    .HasForeignKey(e => e.CourseId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Progress configuration
            modelBuilder.Entity<Progress>(entity =>
            {
                entity.HasKey(e => e.ProgressId);
                entity.HasIndex(e => new { e.StudentId, e.LessonId }).IsUnique();

                entity.HasOne(e => e.Student)
                    .WithMany(u => u.ProgressRecords)
                    .HasForeignKey(e => e.StudentId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Lesson)
                    .WithMany(l => l.ProgressRecords)
                    .HasForeignKey(e => e.LessonId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Review configuration
            modelBuilder.Entity<Review>(entity =>
            {
                entity.HasKey(e => e.ReviewId);
                entity.HasIndex(e => new { e.CourseId, e.StudentId }).IsUnique();
                entity.Property(e => e.Rating).IsRequired();

                entity.HasOne(e => e.Course)
                    .WithMany(c => c.Reviews)
                    .HasForeignKey(e => e.CourseId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Student)
                    .WithMany(u => u.Reviews)
                    .HasForeignKey(e => e.StudentId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // ============================================
            // SEED DATA
            // ============================================

            // Seed Users (Admin, Instructors, Students)
            modelBuilder.Entity<User>().HasData(
                // Admin
                new User
                {
                    UserId = 1,
                    Email = "admin@lms.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                    FirstName = "System",
                    LastName = "Administrator",
                    Role = UserRole.Admin,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                },
                // Instructors
                new User
                {
                    UserId = 2,
                    Email = "john.smith@lms.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Instructor@123"),
                    FirstName = "John",
                    LastName = "Smith",
                    Role = UserRole.Instructor,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                },
                new User
                {
                    UserId = 3,
                    Email = "sarah.johnson@lms.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Instructor@123"),
                    FirstName = "Sarah",
                    LastName = "Johnson",
                    Role = UserRole.Instructor,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                },
                new User
                {
                    UserId = 4,
                    Email = "michael.chen@lms.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Instructor@123"),
                    FirstName = "Michael",
                    LastName = "Chen",
                    Role = UserRole.Instructor,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                },
                // Students
                new User
                {
                    UserId = 5,
                    Email = "emma.davis@student.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Student@123"),
                    FirstName = "Emma",
                    LastName = "Davis",
                    Role = UserRole.Student,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                },
                new User
                {
                    UserId = 6,
                    Email = "james.wilson@student.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Student@123"),
                    FirstName = "James",
                    LastName = "Wilson",
                    Role = UserRole.Student,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                }
            );

            // Seed Categories
            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryId = 1, Name = "Web Development", Description = "Learn web technologies and frameworks", CreatedAt = DateTime.UtcNow },
                new Category { CategoryId = 2, Name = "Mobile Development", Description = "Build mobile applications", CreatedAt = DateTime.UtcNow },
                new Category { CategoryId = 3, Name = "Data Science", Description = "Master data analysis and machine learning", CreatedAt = DateTime.UtcNow },
                new Category { CategoryId = 4, Name = "Cloud Computing", Description = "Learn cloud platforms and services", CreatedAt = DateTime.UtcNow },
                new Category { CategoryId = 5, Name = "DevOps", Description = "CI/CD and infrastructure automation", CreatedAt = DateTime.UtcNow },
                new Category { CategoryId = 6, Name = "Design", Description = "UI/UX and graphic design", CreatedAt = DateTime.UtcNow },
                new Category { CategoryId = 7, Name = "Business", Description = "Management and entrepreneurship", CreatedAt = DateTime.UtcNow }
            );

            // Seed Courses with Real Image URLs
            modelBuilder.Entity<Course>().HasData(
                // Web Development Courses
                new Course
                {
                    CourseId = 1,
                    Title = "Complete React Developer Course",
                    Description = "Master React.js by building real-world projects. Learn hooks, context API, Redux, and modern React patterns.",
                    InstructorId = 2,
                    CategoryId = 1,
                    ThumbnailUrl = "https://images.unsplash.com/photo-1633356122544-f134324a6cee?w=800&q=80",
                    IsPublished = true,
                    Duration = 40,
                    Level = CourseLevel.Intermediate,
                    Price = 89.99m,
                    CreatedAt = DateTime.UtcNow
                },
                new Course
                {
                    CourseId = 2,
                    Title = "ASP.NET Core Web API Development",
                    Description = "Build robust RESTful APIs with ASP.NET Core, Entity Framework, and JWT authentication.",
                    InstructorId = 2,
                    CategoryId = 1,
                    ThumbnailUrl = "https://images.unsplash.com/photo-1516116216624-53e697fedbea?w=800&q=80",
                    IsPublished = true,
                    Duration = 35,
                    Level = CourseLevel.Advanced,
                    Price = 99.99m,
                    CreatedAt = DateTime.UtcNow
                },
                new Course
                {
                    CourseId = 3,
                    Title = "Full-Stack JavaScript Bootcamp",
                    Description = "Learn Node.js, Express, MongoDB, and React to become a full-stack developer.",
                    InstructorId = 3,
                    CategoryId = 1,
                    ThumbnailUrl = "https://images.unsplash.com/photo-1627398242454-45a1465c2479?w=800&q=80",
                    IsPublished = true,
                    Duration = 60,
                    Level = CourseLevel.Beginner,
                    Price = 129.99m,
                    CreatedAt = DateTime.UtcNow
                },
                // Mobile Development
                new Course
                {
                    CourseId = 4,
                    Title = "iOS App Development with Swift",
                    Description = "Build native iOS applications using Swift and SwiftUI. From basics to App Store deployment.",
                    InstructorId = 3,
                    CategoryId = 2,
                    ThumbnailUrl = "https://images.unsplash.com/photo-1551650975-87deedd944c3?w=800&q=80",
                    IsPublished = true,
                    Duration = 45,
                    Level = CourseLevel.Intermediate,
                    Price = 94.99m,
                    CreatedAt = DateTime.UtcNow
                },
                new Course
                {
                    CourseId = 5,
                    Title = "Flutter & Dart - Complete Guide",
                    Description = "Create beautiful cross-platform mobile apps with Flutter. One codebase for iOS and Android.",
                    InstructorId = 4,
                    CategoryId = 2,
                    ThumbnailUrl = "https://images.unsplash.com/photo-1512941937669-90a1b58e7e9c?w=800&q=80",
                    IsPublished = true,
                    Duration = 50,
                    Level = CourseLevel.Beginner,
                    Price = 84.99m,
                    CreatedAt = DateTime.UtcNow
                },
                // Data Science
                new Course
                {
                    CourseId = 6,
                    Title = "Python for Data Science and Machine Learning",
                    Description = "Learn Python, NumPy, Pandas, Matplotlib, Scikit-Learn, and build ML models.",
                    InstructorId = 4,
                    CategoryId = 3,
                    ThumbnailUrl = "https://images.unsplash.com/photo-1551288049-bebda4e38f71?w=800&q=80",
                    IsPublished = true,
                    Duration = 55,
                    Level = CourseLevel.Intermediate,
                    Price = 119.99m,
                    CreatedAt = DateTime.UtcNow
                },
                new Course
                {
                    CourseId = 7,
                    Title = "Deep Learning with TensorFlow",
                    Description = "Master neural networks, CNNs, RNNs, and transformers using TensorFlow and Keras.",
                    InstructorId = 2,
                    CategoryId = 3,
                    ThumbnailUrl = "https://images.unsplash.com/photo-1555949963-aa79dcee981c?w=800&q=80",
                    IsPublished = true,
                    Duration = 48,
                    Level = CourseLevel.Advanced,
                    Price = 139.99m,
                    CreatedAt = DateTime.UtcNow
                },
                // Cloud Computing
                new Course
                {
                    CourseId = 8,
                    Title = "AWS Certified Solutions Architect",
                    Description = "Prepare for AWS certification and learn cloud architecture best practices.",
                    InstructorId = 3,
                    CategoryId = 4,
                    ThumbnailUrl = "https://images.unsplash.com/photo-1451187580459-43490279c0fa?w=800&q=80",
                    IsPublished = true,
                    Duration = 42,
                    Level = CourseLevel.Intermediate,
                    Price = 109.99m,
                    CreatedAt = DateTime.UtcNow
                },
                new Course
                {
                    CourseId = 9,
                    Title = "Microsoft Azure Fundamentals",
                    Description = "Get started with Azure services, deployment, and cloud solutions.",
                    InstructorId = 2,
                    CategoryId = 4,
                    ThumbnailUrl = "https://images.unsplash.com/photo-1544197150-b99a580bb7a8?w=800&q=80",
                    IsPublished = true,
                    Duration = 30,
                    Level = CourseLevel.Beginner,
                    Price = 69.99m,
                    CreatedAt = DateTime.UtcNow
                },
                // DevOps
                new Course
                {
                    CourseId = 10,
                    Title = "Docker and Kubernetes Mastery",
                    Description = "Containerize applications and orchestrate them with Kubernetes in production.",
                    InstructorId = 4,
                    CategoryId = 5,
                    ThumbnailUrl = "https://images.unsplash.com/photo-1605745341112-85968b19335b?w=800&q=80",
                    IsPublished = true,
                    Duration = 38,
                    Level = CourseLevel.Advanced,
                    Price = 99.99m,
                    CreatedAt = DateTime.UtcNow
                },
                // Design
                new Course
                {
                    CourseId = 11,
                    Title = "UI/UX Design Fundamentals",
                    Description = "Learn user interface and experience design principles using Figma.",
                    InstructorId = 3,
                    CategoryId = 6,
                    ThumbnailUrl = "https://images.unsplash.com/photo-1561070791-2526d30994b5?w=800&q=80",
                    IsPublished = true,
                    Duration = 28,
                    Level = CourseLevel.Beginner,
                    Price = 59.99m,
                    CreatedAt = DateTime.UtcNow
                },
                // Business
                new Course
                {
                    CourseId = 12,
                    Title = "Digital Marketing Masterclass",
                    Description = "Master SEO, social media marketing, email campaigns, and analytics.",
                    InstructorId = 2,
                    CategoryId = 7,
                    ThumbnailUrl = "https://images.unsplash.com/photo-1460925895917-afdab827c52f?w=800&q=80",
                    IsPublished = true,
                    Duration = 32,
                    Level = CourseLevel.Beginner,
                    Price = 74.99m,
                    CreatedAt = DateTime.UtcNow
                },
                // Free Courses
                new Course
                {
                    CourseId = 13,
                    Title = "Introduction to Programming with Python",
                    Description = "Start your coding journey with Python. Perfect for absolute beginners.",
                    InstructorId = 4,
                    CategoryId = 3,
                    ThumbnailUrl = "https://images.unsplash.com/photo-1526374965328-7f61d4dc18c5?w=800&q=80",
                    IsPublished = true,
                    Duration = 20,
                    Level = CourseLevel.Beginner,
                    Price = 0m,
                    CreatedAt = DateTime.UtcNow
                },
                new Course
                {
                    CourseId = 14,
                    Title = "Git and GitHub Essentials",
                    Description = "Learn version control with Git and collaborate using GitHub.",
                    InstructorId = 3,
                    CategoryId = 5,
                    ThumbnailUrl = "https://images.unsplash.com/photo-1556075798-4825dfaaf498?w=800&q=80",
                    IsPublished = true,
                    Duration = 15,
                    Level = CourseLevel.Beginner,
                    Price = 0m,
                    CreatedAt = DateTime.UtcNow
                }
            );

            // Seed Sections for Course 1 (Complete React Developer)
            modelBuilder.Entity<Section>().HasData(
                new Section { SectionId = 1, CourseId = 1, Title = "React Fundamentals", Description = "Learn the basics of React", OrderIndex = 1, CreatedAt = DateTime.UtcNow },
                new Section { SectionId = 2, CourseId = 1, Title = "Advanced React Patterns", Description = "Master advanced concepts", OrderIndex = 2, CreatedAt = DateTime.UtcNow },
                new Section { SectionId = 3, CourseId = 1, Title = "State Management with Redux", Description = "Learn Redux for complex apps", OrderIndex = 3, CreatedAt = DateTime.UtcNow }
            );

            // Seed Lessons for Section 1
            modelBuilder.Entity<Lesson>().HasData(
                new Lesson
                {
                    LessonId = 1,
                    SectionId = 1,
                    Title = "Introduction to React",
                    ContentType = ContentType.Video,
                    ContentUrl = "https://example.com/videos/react-intro",
                    Duration = 15,
                    OrderIndex = 1,
                    IsFree = true,
                    CreatedAt = DateTime.UtcNow
                },
                new Lesson
                {
                    LessonId = 2,
                    SectionId = 1,
                    Title = "Components and Props",
                    ContentType = ContentType.Video,
                    ContentUrl = "https://example.com/videos/components-props",
                    Duration = 20,
                    OrderIndex = 2,
                    IsFree = false,
                    CreatedAt = DateTime.UtcNow
                },
                new Lesson
                {
                    LessonId = 3,
                    SectionId = 1,
                    Title = "State and Lifecycle",
                    ContentType = ContentType.Video,
                    ContentUrl = "https://example.com/videos/state-lifecycle",
                    Duration = 25,
                    OrderIndex = 3,
                    IsFree = false,
                    CreatedAt = DateTime.UtcNow
                }
            );

            // Seed Enrollments
            modelBuilder.Entity<Enrollment>().HasData(
                new Enrollment
                {
                    EnrollmentId = 1,
                    StudentId = 5,
                    CourseId = 1,
                    EnrolledAt = DateTime.UtcNow.AddDays(-30),
                    Status = EnrollmentStatus.Active,
                    ProgressPercentage = 45.5m
                },
                new Enrollment
                {
                    EnrollmentId = 2,
                    StudentId = 5,
                    CourseId = 3,
                    EnrolledAt = DateTime.UtcNow.AddDays(-20),
                    Status = EnrollmentStatus.Active,
                    ProgressPercentage = 20.0m
                },
                new Enrollment
                {
                    EnrollmentId = 3,
                    StudentId = 6,
                    CourseId = 1,
                    EnrolledAt = DateTime.UtcNow.AddDays(-15),
                    Status = EnrollmentStatus.Active,
                    ProgressPercentage = 60.0m
                },
                new Enrollment
                {
                    EnrollmentId = 4,
                    StudentId = 6,
                    CourseId = 13,
                    EnrolledAt = DateTime.UtcNow.AddDays(-10),
                    Status = EnrollmentStatus.Completed,
                    ProgressPercentage = 100.0m,
                    CompletedAt = DateTime.UtcNow.AddDays(-2)
                }
            );

            // Seed Progress Records
            modelBuilder.Entity<Progress>().HasData(
                new Progress
                {
                    ProgressId = 1,
                    StudentId = 5,
                    LessonId = 1,
                    IsCompleted = true,
                    LastAccessedAt = DateTime.UtcNow.AddDays(-5),
                    CompletedAt = DateTime.UtcNow.AddDays(-5)
                },
                new Progress
                {
                    ProgressId = 2,
                    StudentId = 5,
                    LessonId = 2,
                    IsCompleted = true,
                    LastAccessedAt = DateTime.UtcNow.AddDays(-3),
                    CompletedAt = DateTime.UtcNow.AddDays(-3)
                },
                new Progress
                {
                    ProgressId = 3,
                    StudentId = 6,
                    LessonId = 1,
                    IsCompleted = true,
                    LastAccessedAt = DateTime.UtcNow.AddDays(-4),
                    CompletedAt = DateTime.UtcNow.AddDays(-4)
                }
            );

            // Seed Reviews
            modelBuilder.Entity<Review>().HasData(
                new Review
                {
                    ReviewId = 1,
                    CourseId = 1,
                    StudentId = 6,
                    Rating = 5,
                    Comment = "Excellent course! Very well structured and easy to follow.",
                    CreatedAt = DateTime.UtcNow.AddDays(-7)
                },
                new Review
                {
                    ReviewId = 2,
                    CourseId = 3,
                    StudentId = 5,
                    Rating = 4,
                    Comment = "Great content, but could use more practical examples.",
                    CreatedAt = DateTime.UtcNow.AddDays(-5)
                },
                new Review
                {
                    ReviewId = 3,
                    CourseId = 13,
                    StudentId = 6,
                    Rating = 5,
                    Comment = "Perfect for beginners! Highly recommended.",
                    CreatedAt = DateTime.UtcNow.AddDays(-2)
                }
            );
        }
    }
}
