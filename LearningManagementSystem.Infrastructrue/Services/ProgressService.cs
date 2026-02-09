using LearningManagementSystem.Core.DTOs.Common;
using LearningManagementSystem.Core.Interfaces;
using LearningManagementSystem.Core.Interfaces.Services;
using LearningManagementSystem.Core.Models.Entities;
using LearningManagementSystem.Core.Models.Enums;

namespace LearningManagementSystem.Infrastructrue.Services
{
    public class ProgressService : IProgressService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProgressService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResultDto<bool>> MarkLessonCompleteAsync(int studentId, int lessonId)
        {
            // Check if student is enrolled in the course
            var lesson = await _unitOfWork.Lessons.GetByIdAsync(lessonId);
            if (lesson == null)
                return ResultDto<bool>.FailureResult("Lesson not found");

            var section = await _unitOfWork.Sections.GetByIdAsync(lesson.SectionId);
            var isEnrolled = await _unitOfWork.Enrollments
          .AnyAsync(e => e.StudentId == studentId && e.CourseId == section!.CourseId && e.Status == EnrollmentStatus.Active);

            if (!isEnrolled)
                return ResultDto<bool>.FailureResult("Not enrolled in this course");

            // Check if progress already exists
            var existingProgress = await _unitOfWork.ProgressRecords
                .FirstOrDefaultAsync(p => p.StudentId == studentId && p.LessonId == lessonId);

            if (existingProgress != null)
            {
                existingProgress.IsCompleted = true;
                existingProgress.CompletedAt = DateTime.UtcNow;
                existingProgress.LastAccessedAt = DateTime.UtcNow;
                _unitOfWork.ProgressRecords.Update(existingProgress);
            }
            else
            {
                var progress = new Progress
                {
                    StudentId = studentId,
                    LessonId = lessonId,
                    IsCompleted = true,
                    CompletedAt = DateTime.UtcNow,
                    LastAccessedAt = DateTime.UtcNow
                };
                await _unitOfWork.ProgressRecords.AddAsync(progress);
            }

            await _unitOfWork.SaveChangesAsync();

            // Update enrollment progress percentage
            await UpdateEnrollmentProgressAsync(studentId, section!.CourseId);

            return ResultDto<bool>.SuccessResult(true, "Lesson marked as complete");
        }

        public async Task<ResultDto<decimal>> CalculateCourseProgressAsync(int studentId, int courseId)
        {
            // Get all lessons in course
            var sections = await _unitOfWork.Sections.FindAsync(s => s.CourseId == courseId);
            var sectionIds = sections.Select(s => s.SectionId).ToList();

            var allLessons = new List<Lesson>();
            foreach (var sectionId in sectionIds)
            {
                var lessons = await _unitOfWork.Lessons.FindAsync(l => l.SectionId == sectionId);
                allLessons.AddRange(lessons);
            }

            var totalLessons = allLessons.Count;
            if (totalLessons == 0)
                return ResultDto<decimal>.SuccessResult(0);

            // Get completed lessons
            var completedLessons = await _unitOfWork.ProgressRecords
        .CountAsync(p => p.StudentId == studentId && p.IsCompleted && allLessons.Select(l => l.LessonId).Contains(p.LessonId));

            var progressPercentage = (decimal)completedLessons / totalLessons * 100;
            return ResultDto<decimal>.SuccessResult(progressPercentage);
        }

        public async Task<ResultDto<int?>> GetLastAccessedLessonAsync(int studentId, int courseId)
        {
            var sections = await _unitOfWork.Sections.FindAsync(s => s.CourseId == courseId);
            var sectionIds = sections.Select(s => s.SectionId).ToList();

            var progressRecords = await _unitOfWork.ProgressRecords
                 .FindAsync(p => p.StudentId == studentId);

            var lastAccessed = progressRecords
 .Where(p => sectionIds.Contains(p.Lesson.SectionId))
         .OrderByDescending(p => p.LastAccessedAt)
             .FirstOrDefault();

            return ResultDto<int?>.SuccessResult(lastAccessed?.LessonId);
        }

        private async Task UpdateEnrollmentProgressAsync(int studentId, int courseId)
        {
            var enrollment = await _unitOfWork.Enrollments
             .FirstOrDefaultAsync(e => e.StudentId == studentId && e.CourseId == courseId);

            if (enrollment == null) return;

            var progressResult = await CalculateCourseProgressAsync(studentId, courseId);
            enrollment.ProgressPercentage = progressResult.Data;

            if (enrollment.ProgressPercentage >= 100)
            {
                enrollment.Status = EnrollmentStatus.Completed;
                enrollment.CompletedAt = DateTime.UtcNow;
            }

            _unitOfWork.Enrollments.Update(enrollment);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
