using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LearningManagementSystem.Infrastructrue.Migrations
{
    /// <inheritdoc />
    public partial class SeedInitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "CreatedAt", "Description", "Name" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 2, 12, 14, 11, 38, 875, DateTimeKind.Utc).AddTicks(2798), "Learn web technologies and frameworks", "Web Development" },
                    { 2, new DateTime(2026, 2, 12, 14, 11, 38, 875, DateTimeKind.Utc).AddTicks(2801), "Build mobile applications", "Mobile Development" },
                    { 3, new DateTime(2026, 2, 12, 14, 11, 38, 875, DateTimeKind.Utc).AddTicks(2802), "Master data analysis and machine learning", "Data Science" },
                    { 4, new DateTime(2026, 2, 12, 14, 11, 38, 875, DateTimeKind.Utc).AddTicks(2803), "Learn cloud platforms and services", "Cloud Computing" },
                    { 5, new DateTime(2026, 2, 12, 14, 11, 38, 875, DateTimeKind.Utc).AddTicks(2804), "CI/CD and infrastructure automation", "DevOps" },
                    { 6, new DateTime(2026, 2, 12, 14, 11, 38, 875, DateTimeKind.Utc).AddTicks(2806), "UI/UX and graphic design", "Design" },
                    { 7, new DateTime(2026, 2, 12, 14, 11, 38, 875, DateTimeKind.Utc).AddTicks(2807), "Management and entrepreneurship", "Business" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "CreatedAt", "Email", "FirstName", "IsActive", "LastName", "PasswordHash", "Role", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 2, 12, 14, 11, 38, 299, DateTimeKind.Utc).AddTicks(1414), "admin@lms.com", "System", true, "Administrator", "$2a$11$j.4/EqcFmNr1RSnBWZaazOH5R2fjMtdtm0QmtoGgh.QjnK3MQV0Mu", 1, null },
                    { 2, new DateTime(2026, 2, 12, 14, 11, 38, 414, DateTimeKind.Utc).AddTicks(4518), "john.smith@lms.com", "John", true, "Smith", "$2a$11$vk8m0hkKQzEenZG08bDQRuPwqavKPQsp9xzB3XOGXJOv./BAquUtG", 2, null },
                    { 3, new DateTime(2026, 2, 12, 14, 11, 38, 528, DateTimeKind.Utc).AddTicks(9264), "sarah.johnson@lms.com", "Sarah", true, "Johnson", "$2a$11$NjpWxJGga3NObmPY5JPRBemZny2Gc.QsFKYbC.QTmcT5tIxRhTtPG", 2, null },
                    { 4, new DateTime(2026, 2, 12, 14, 11, 38, 644, DateTimeKind.Utc).AddTicks(1711), "michael.chen@lms.com", "Michael", true, "Chen", "$2a$11$90j8wYv4DPR8H66P1CrdEerISFGaeNaaZxfmEmQvTuYafP5e0Mea2", 2, null },
                    { 5, new DateTime(2026, 2, 12, 14, 11, 38, 759, DateTimeKind.Utc).AddTicks(7294), "emma.davis@student.com", "Emma", true, "Davis", "$2a$11$lGkGl184HzmVxKc3MDtzD.rHflD6yM9ljYjC2mUYtEuAqemtx.jM2", 3, null },
                    { 6, new DateTime(2026, 2, 12, 14, 11, 38, 875, DateTimeKind.Utc).AddTicks(1835), "james.wilson@student.com", "James", true, "Wilson", "$2a$11$AxUl0tM8IPkqG9E5N4X/aeGsnksjFyJdjjTFpGJZEY9BeEebE5Upm", 3, null }
                });

            migrationBuilder.InsertData(
                table: "Courses",
                columns: new[] { "CourseId", "CategoryId", "CreatedAt", "Description", "Duration", "InstructorId", "IsPublished", "Level", "Price", "ThumbnailUrl", "Title", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2026, 2, 12, 14, 11, 38, 875, DateTimeKind.Utc).AddTicks(2853), "Master React.js by building real-world projects. Learn hooks, context API, Redux, and modern React patterns.", 40, 2, true, 2, 89.99m, "https://images.unsplash.com/photo-1633356122544-f134324a6cee?w=800&q=80", "Complete React Developer Course", null },
                    { 2, 1, new DateTime(2026, 2, 12, 14, 11, 38, 875, DateTimeKind.Utc).AddTicks(2857), "Build robust RESTful APIs with ASP.NET Core, Entity Framework, and JWT authentication.", 35, 2, true, 3, 99.99m, "https://images.unsplash.com/photo-1516116216624-53e697fedbea?w=800&q=80", "ASP.NET Core Web API Development", null },
                    { 3, 1, new DateTime(2026, 2, 12, 14, 11, 38, 875, DateTimeKind.Utc).AddTicks(2859), "Learn Node.js, Express, MongoDB, and React to become a full-stack developer.", 60, 3, true, 1, 129.99m, "https://images.unsplash.com/photo-1627398242454-45a1465c2479?w=800&q=80", "Full-Stack JavaScript Bootcamp", null },
                    { 4, 2, new DateTime(2026, 2, 12, 14, 11, 38, 875, DateTimeKind.Utc).AddTicks(2861), "Build native iOS applications using Swift and SwiftUI. From basics to App Store deployment.", 45, 3, true, 2, 94.99m, "https://images.unsplash.com/photo-1551650975-87deedd944c3?w=800&q=80", "iOS App Development with Swift", null },
                    { 5, 2, new DateTime(2026, 2, 12, 14, 11, 38, 875, DateTimeKind.Utc).AddTicks(2921), "Create beautiful cross-platform mobile apps with Flutter. One codebase for iOS and Android.", 50, 4, true, 1, 84.99m, "https://images.unsplash.com/photo-1512941937669-90a1b58e7e9c?w=800&q=80", "Flutter & Dart - Complete Guide", null },
                    { 6, 3, new DateTime(2026, 2, 12, 14, 11, 38, 875, DateTimeKind.Utc).AddTicks(2924), "Learn Python, NumPy, Pandas, Matplotlib, Scikit-Learn, and build ML models.", 55, 4, true, 2, 119.99m, "https://images.unsplash.com/photo-1551288049-bebda4e38f71?w=800&q=80", "Python for Data Science and Machine Learning", null },
                    { 7, 3, new DateTime(2026, 2, 12, 14, 11, 38, 875, DateTimeKind.Utc).AddTicks(2926), "Master neural networks, CNNs, RNNs, and transformers using TensorFlow and Keras.", 48, 2, true, 3, 139.99m, "https://images.unsplash.com/photo-1555949963-aa79dcee981c?w=800&q=80", "Deep Learning with TensorFlow", null },
                    { 8, 4, new DateTime(2026, 2, 12, 14, 11, 38, 875, DateTimeKind.Utc).AddTicks(2929), "Prepare for AWS certification and learn cloud architecture best practices.", 42, 3, true, 2, 109.99m, "https://images.unsplash.com/photo-1451187580459-43490279c0fa?w=800&q=80", "AWS Certified Solutions Architect", null },
                    { 9, 4, new DateTime(2026, 2, 12, 14, 11, 38, 875, DateTimeKind.Utc).AddTicks(2932), "Get started with Azure services, deployment, and cloud solutions.", 30, 2, true, 1, 69.99m, "https://images.unsplash.com/photo-1544197150-b99a580bb7a8?w=800&q=80", "Microsoft Azure Fundamentals", null },
                    { 10, 5, new DateTime(2026, 2, 12, 14, 11, 38, 875, DateTimeKind.Utc).AddTicks(2934), "Containerize applications and orchestrate them with Kubernetes in production.", 38, 4, true, 3, 99.99m, "https://images.unsplash.com/photo-1605745341112-85968b19335b?w=800&q=80", "Docker and Kubernetes Mastery", null },
                    { 11, 6, new DateTime(2026, 2, 12, 14, 11, 38, 875, DateTimeKind.Utc).AddTicks(2936), "Learn user interface and experience design principles using Figma.", 28, 3, true, 1, 59.99m, "https://images.unsplash.com/photo-1561070791-2526d30994b5?w=800&q=80", "UI/UX Design Fundamentals", null },
                    { 12, 7, new DateTime(2026, 2, 12, 14, 11, 38, 875, DateTimeKind.Utc).AddTicks(2938), "Master SEO, social media marketing, email campaigns, and analytics.", 32, 2, true, 1, 74.99m, "https://images.unsplash.com/photo-1460925895917-afdab827c52f?w=800&q=80", "Digital Marketing Masterclass", null },
                    { 13, 3, new DateTime(2026, 2, 12, 14, 11, 38, 875, DateTimeKind.Utc).AddTicks(2942), "Start your coding journey with Python. Perfect for absolute beginners.", 20, 4, true, 1, 0m, "https://images.unsplash.com/photo-1526374965328-7f61d4dc18c5?w=800&q=80", "Introduction to Programming with Python", null },
                    { 14, 5, new DateTime(2026, 2, 12, 14, 11, 38, 875, DateTimeKind.Utc).AddTicks(2944), "Learn version control with Git and collaborate using GitHub.", 15, 3, true, 1, 0m, "https://images.unsplash.com/photo-1556075798-4825dfaaf498?w=800&q=80", "Git and GitHub Essentials", null }
                });

            migrationBuilder.InsertData(
                table: "Enrollments",
                columns: new[] { "EnrollmentId", "CompletedAt", "CourseId", "EnrolledAt", "ProgressPercentage", "Status", "StudentId" },
                values: new object[,]
                {
                    { 1, null, 1, new DateTime(2026, 1, 13, 14, 11, 38, 875, DateTimeKind.Utc).AddTicks(3063), 45.5m, 1, 5 },
                    { 2, null, 3, new DateTime(2026, 1, 23, 14, 11, 38, 875, DateTimeKind.Utc).AddTicks(3073), 20.0m, 1, 5 },
                    { 3, null, 1, new DateTime(2026, 1, 28, 14, 11, 38, 875, DateTimeKind.Utc).AddTicks(3074), 60.0m, 1, 6 },
                    { 4, new DateTime(2026, 2, 10, 14, 11, 38, 875, DateTimeKind.Utc).AddTicks(3076), 13, new DateTime(2026, 2, 2, 14, 11, 38, 875, DateTimeKind.Utc).AddTicks(3076), 100.0m, 2, 6 }
                });

            migrationBuilder.InsertData(
                table: "Reviews",
                columns: new[] { "ReviewId", "Comment", "CourseId", "CreatedAt", "Rating", "StudentId", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "Excellent course! Very well structured and easy to follow.", 1, new DateTime(2026, 2, 5, 14, 11, 38, 875, DateTimeKind.Utc).AddTicks(3140), 5, 6, null },
                    { 2, "Great content, but could use more practical examples.", 3, new DateTime(2026, 2, 7, 14, 11, 38, 875, DateTimeKind.Utc).AddTicks(3142), 4, 5, null },
                    { 3, "Perfect for beginners! Highly recommended.", 13, new DateTime(2026, 2, 10, 14, 11, 38, 875, DateTimeKind.Utc).AddTicks(3143), 5, 6, null }
                });

            migrationBuilder.InsertData(
                table: "Sections",
                columns: new[] { "SectionId", "CourseId", "CreatedAt", "Description", "OrderIndex", "Title" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2026, 2, 12, 14, 11, 38, 875, DateTimeKind.Utc).AddTicks(2996), "Learn the basics of React", 1, "React Fundamentals" },
                    { 2, 1, new DateTime(2026, 2, 12, 14, 11, 38, 875, DateTimeKind.Utc).AddTicks(2998), "Master advanced concepts", 2, "Advanced React Patterns" },
                    { 3, 1, new DateTime(2026, 2, 12, 14, 11, 38, 875, DateTimeKind.Utc).AddTicks(2999), "Learn Redux for complex apps", 3, "State Management with Redux" }
                });

            migrationBuilder.InsertData(
                table: "Lessons",
                columns: new[] { "LessonId", "ContentType", "ContentUrl", "CreatedAt", "Duration", "IsFree", "OrderIndex", "SectionId", "TextContent", "Title" },
                values: new object[,]
                {
                    { 1, 1, "https://example.com/videos/react-intro", new DateTime(2026, 2, 12, 14, 11, 38, 875, DateTimeKind.Utc).AddTicks(3031), 15, true, 1, 1, null, "Introduction to React" },
                    { 2, 1, "https://example.com/videos/components-props", new DateTime(2026, 2, 12, 14, 11, 38, 875, DateTimeKind.Utc).AddTicks(3033), 20, false, 2, 1, null, "Components and Props" },
                    { 3, 1, "https://example.com/videos/state-lifecycle", new DateTime(2026, 2, 12, 14, 11, 38, 875, DateTimeKind.Utc).AddTicks(3035), 25, false, 3, 1, null, "State and Lifecycle" }
                });

            migrationBuilder.InsertData(
                table: "ProgressRecords",
                columns: new[] { "ProgressId", "CompletedAt", "IsCompleted", "LastAccessedAt", "LessonId", "StudentId" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 2, 7, 14, 11, 38, 875, DateTimeKind.Utc).AddTicks(3109), true, new DateTime(2026, 2, 7, 14, 11, 38, 875, DateTimeKind.Utc).AddTicks(3108), 1, 5 },
                    { 2, new DateTime(2026, 2, 9, 14, 11, 38, 875, DateTimeKind.Utc).AddTicks(3111), true, new DateTime(2026, 2, 9, 14, 11, 38, 875, DateTimeKind.Utc).AddTicks(3110), 2, 5 },
                    { 3, new DateTime(2026, 2, 8, 14, 11, 38, 875, DateTimeKind.Utc).AddTicks(3112), true, new DateTime(2026, 2, 8, 14, 11, 38, 875, DateTimeKind.Utc).AddTicks(3112), 1, 6 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "CourseId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "CourseId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "CourseId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "CourseId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "CourseId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "CourseId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "CourseId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "CourseId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "CourseId",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "CourseId",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "CourseId",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Enrollments",
                keyColumn: "EnrollmentId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Enrollments",
                keyColumn: "EnrollmentId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Enrollments",
                keyColumn: "EnrollmentId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Enrollments",
                keyColumn: "EnrollmentId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Lessons",
                keyColumn: "LessonId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ProgressRecords",
                keyColumn: "ProgressId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ProgressRecords",
                keyColumn: "ProgressId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ProgressRecords",
                keyColumn: "ProgressId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Reviews",
                keyColumn: "ReviewId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Reviews",
                keyColumn: "ReviewId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Reviews",
                keyColumn: "ReviewId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "SectionId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "SectionId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "CourseId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "CourseId",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Lessons",
                keyColumn: "LessonId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Lessons",
                keyColumn: "LessonId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "SectionId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "CourseId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2);
        }
    }
}
