-- Create Test Enrollments for Students
-- Run this in SQL Server Management Studio

USE LMS_DB;
GO

PRINT '=== Creating Test Enrollments for Students ===';
PRINT '';

-- Check if we have students
DECLARE @StudentCount INT = (SELECT COUNT(*) FROM Users WHERE Role = 'Student');
PRINT 'Students in database: ' + CAST(@StudentCount AS VARCHAR);

IF @StudentCount = 0
BEGIN
    PRINT 'ERROR: No students found! Please register a student first.';
    RETURN;
END

-- Check if we have published courses
DECLARE @CourseCount INT = (SELECT COUNT(*) FROM Courses WHERE IsPublished = 1);
PRINT 'Published courses: ' + CAST(@CourseCount AS VARCHAR);

IF @CourseCount = 0
BEGIN
    PRINT 'ERROR: No published courses found!';
    RETURN;
END

PRINT '';
PRINT '--- Creating Enrollments ---';
PRINT '';

-- Get first student
DECLARE @StudentId INT = (SELECT TOP 1 UserId FROM Users WHERE Role = 'Student' ORDER BY UserId);
DECLARE @StudentEmail VARCHAR(255) = (SELECT Email FROM Users WHERE UserId = @StudentId);

PRINT 'Enrolling student: ' + @StudentEmail + ' (ID: ' + CAST(@StudentId AS VARCHAR) + ')';
PRINT '';

-- Enroll in first 3 published courses
DECLARE @CourseId INT;
DECLARE @CourseTitle VARCHAR(200);
DECLARE @Counter INT = 0;

DECLARE course_cursor CURSOR FOR
SELECT TOP 3 CourseId, Title 
FROM Courses 
WHERE IsPublished = 1
ORDER BY CourseId;

OPEN course_cursor;
FETCH NEXT FROM course_cursor INTO @CourseId, @CourseTitle;

WHILE @@FETCH_STATUS = 0 AND @Counter < 3
BEGIN
    -- Check if already enrolled
    IF NOT EXISTS (SELECT 1 FROM Enrollments WHERE StudentId = @StudentId AND CourseId = @CourseId)
    BEGIN
        -- Insert enrollment with varying progress
        DECLARE @Progress DECIMAL(5,2);
        
        IF @Counter = 0
       SET @Progress = 0;      -- Not started
  ELSE IF @Counter = 1
       SET @Progress = 45;   -- In progress
        ELSE
     SET @Progress = 100;    -- Completed
        
        INSERT INTO Enrollments (StudentId, CourseId, EnrolledAt, Status, ProgressPercentage, CompletedAt)
     VALUES (
      @StudentId, 
            @CourseId, 
   DATEADD(DAY, -@Counter * 7, GETDATE()), -- Enrolled 0, 7, 14 days ago
       CASE WHEN @Progress = 100 THEN 'Completed' ELSE 'Active' END,
   @Progress,
  CASE WHEN @Progress = 100 THEN GETDATE() ELSE NULL END
        );
        
   PRINT '  ? Enrolled in: ' + @CourseTitle + ' (Progress: ' + CAST(@Progress AS VARCHAR) + '%)';
    END
    ELSE
    BEGIN
   PRINT '  ? Already enrolled in: ' + @CourseTitle;
    END
    
    SET @Counter = @Counter + 1;
    FETCH NEXT FROM course_cursor INTO @CourseId, @CourseTitle;
END

CLOSE course_cursor;
DEALLOCATE course_cursor;

PRINT '';
PRINT '=== Summary ===';
PRINT '';

-- Show enrollments
SELECT 
    u.Email as StudentEmail,
    c.Title as CourseTitle,
    e.Status,
    e.ProgressPercentage as Progress,
    e.EnrolledAt,
    e.CompletedAt
FROM Enrollments e
INNER JOIN Users u ON e.StudentId = u.UserId
INNER JOIN Courses c ON e.CourseId = c.CourseId
WHERE u.UserId = @StudentId
ORDER BY e.EnrolledAt DESC;

PRINT '';
PRINT '? Test enrollments created successfully!';
PRINT '';
PRINT 'Next steps:';
PRINT '  1. Restart MVC project (Shift+F5, then F5)';
PRINT '  2. Login with: ' + @StudentEmail;
PRINT '  3. Go to: https://localhost:7012/Student/MyCourses';
PRINT '  4. You should see the enrolled courses!';
PRINT '';
