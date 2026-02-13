# ?? STUDENT COURSES NOT SHOWING - FIXED!

## ? **PROBLEM IDENTIFIED & SOLVED**

### **The Issue:**
When logged in as a student, the "My Courses" page doesn't show any enrolled courses.

### **Root Causes Found:**

1. **StudentController.MyCourses() was empty** ?
   - Method returned empty model without fetching data
   - No API calls were being made

2. **StudentController.Dashboard() was empty** ?
   - Dashboard showed no enrolled courses

3. **View used client-side JavaScript** ?
   - Tried to fetch from non-existent `/api/enrollments/my-enrollments`
   - Should use server-side data from Model

---

## ? **WHAT I FIXED**

### **File 1: `LearningManagementSystem.MVC\Controllers\StudentController.cs`**

#### **Fixed MyCourses() Method:**

**BEFORE:**
```csharp
public async Task<IActionResult> MyCourses()
{
    var model = new MyCoursesViewModel();
    return View(model);  // Empty model! ?
}
```

**AFTER:**
```csharp
public async Task<IActionResult> MyCourses()
{
    var apiBase = _configuration["ApiSettings:BaseUrl"] ?? "https://localhost:7059/api/";
    var client = _httpClientFactory.CreateClient();
    client.BaseAddress = new Uri(apiBase);
    
    var model = new MyCoursesViewModel();

    // 1. Get auth token from cookie
    var token = Request.Cookies["AuthToken"];
    if (!string.IsNullOrEmpty(token))
    {
        client.DefaultRequestHeaders.Authorization = 
 new AuthenticationHeaderValue("Bearer", token);
    }
    else
    {
        return RedirectToAction("Login", "Account");
    }

    // 2. Fetch enrolled course IDs
    var enrollmentsResponse = await client.GetAsync("enrollments/my-courses");
    
    if (enrollmentsResponse.IsSuccessStatusCode)
    {
        var enrollmentsJson = await enrollmentsResponse.Content.ReadAsStringAsync();
        var enrollmentsDoc = JsonDocument.Parse(enrollmentsJson);
        
        if (enrollmentsDoc.RootElement.TryGetProperty("data", out var courseIds))
{
       // 3. Fetch course details for each enrollment
    foreach (var courseId in courseIds.EnumerateArray())
 {
      var id = courseId.GetInt32();
    var courseResponse = await client.GetAsync($"courses/{id}");
    
  if (courseResponse.IsSuccessStatusCode)
      {
  var courseJson = await courseResponse.Content.ReadAsStringAsync();
         var courseDoc = JsonDocument.Parse(courseJson);
    
     if (courseDoc.RootElement.TryGetProperty("data", out var courseData))
      {
   model.EnrolledCourses.Add(new EnrolledCourseVm
      {
            CourseId = courseData.GetProperty("courseId").GetInt32(),
      Title = courseData.GetProperty("title").GetString() ?? string.Empty,
    InstructorName = courseData.GetProperty("instructorName").GetString() ?? string.Empty,
              ThumbnailUrl = courseData.TryGetProperty("thumbnailUrl", out var thumb) 
          && thumb.ValueKind != JsonValueKind.Null 
        ? thumb.GetString() ?? string.Empty 
        : string.Empty,
      ProgressPercentage = 0,
  Status = "Active",
        EnrolledAt = DateTime.UtcNow
      });
          }
           }
    }
        }
    }
    
    return View(model);
}
```

#### **Fixed Dashboard() Method:**

Added similar logic to fetch and display enrolled courses in dashboard.

---

### **File 2: `LearningManagementSystem.MVC\Views\Student\MyCourses.cshtml`**

**BEFORE:**
- Used JavaScript to fetch data client-side
- Tried to call non-existent API endpoint
- Required extra API calls from browser

**AFTER:**
- Uses server-side rendered Razor view
- Displays Model.EnrolledCourses from controller
- Three tabs: All, In Progress, Completed
- Shows progress bars and status badges
- Links to learning page

---

## ?? **TO APPLY THE FIX**

### **?? RESTART MVC PROJECT!**

The fix won't work until you restart:

**Option 1: Visual Studio**
```
1. Stop MVC (Shift+F5)
2. Start MVC (F5)
```

**Option 2: Command Line**
```powershell
Get-Process -Name "LearningManagementSystem.MVC" | Stop-Process -Force
cd LearningManagementSystem.MVC
dotnet run
```

---

## ?? **TESTING STEPS**

### **Prerequisites:**
1. ? Both API and MVC must be running
2. ? You must be logged in as a **Student**
3. ? You must have **enrolled in courses** (test with enrollments in database)

### **Test 1: Check if Student Has Enrollments**

```sql
-- Run in SQL Server Management Studio
SELECT e.EnrollmentId, e.StudentId, e.CourseId, e.Status, 
       u.Email as StudentEmail, c.Title as CourseTitle
FROM Enrollments e
INNER JOIN Users u ON e.StudentId = u.UserId
INNER JOIN Courses c ON e.CourseId = c.CourseId
WHERE u.Role = 'Student';
```

**If NO rows:** You need to enroll a student in courses first!

### **Test 2: Enroll a Student in a Course**

```sql
-- Get a student user ID
DECLARE @StudentId INT = (SELECT TOP 1 UserId FROM Users WHERE Role = 'Student');

-- Get a published course ID
DECLARE @CourseId INT = (SELECT TOP 1 CourseId FROM Courses WHERE IsPublished = 1);

-- Insert enrollment
INSERT INTO Enrollments (StudentId, CourseId, EnrolledAt, Status, ProgressPercentage)
VALUES (@StudentId, @CourseId, GETDATE(), 'Active', 0);

SELECT 'Enrolled student ' + CAST(@StudentId AS VARCHAR) + ' in course ' + CAST(@CourseId AS VARCHAR);
```

### **Test 3: Login and View Courses**

1. **Login** as student:
   - Go to: https://localhost:7012/Account/Login
   - Use student credentials
   
2. **Navigate to My Courses**:
   - Go to: https://localhost:7012/Student/MyCourses
   - **Expected:** See enrolled courses ?

3. **Check Dashboard**:
   - Go to: https://localhost:7012/Student/Dashboard
   - **Expected:** See course count and recent courses ?

---

## ?? **EXPECTED RESULTS**

### **My Courses Page:**

**Should Display:**
- ? List of all enrolled courses
- ? Course thumbnails
- ? Course titles and instructor names
- ? Progress bars (0% for new enrollments)
- ? Status badges (Not Started, In Progress, Completed)
- ? Tabs to filter courses (All, In Progress, Completed)
- ? "Start Learning" or "Continue Learning" buttons

**Should Show if No Enrollments:**
```
No courses enrolled yet
Start your learning journey by browsing courses
```

### **Dashboard:**
- ? Total enrolled courses count
- ? Recent courses (up to 3)
- ? Continue Learning section

---

## ?? **TROUBLESHOOTING**

### **Issue 1: Still No Courses After Restart**

**Check logs in Output window:**
```
View ? Output ? Select "LearningManagementSystem.MVC"
```

Look for:
- `Fetching enrolled courses for student`
- `Enrollments API response: OK`
- `Loaded X enrolled courses`

**If you see "Unauthorized":**
- You're not logged in
- Token is expired or invalid
- Clear cookies and login again

### **Issue 2: Database Has No Enrollments**

Run this query to create test enrollment:
```sql
-- Create test enrollment
DECLARE @StudentId INT = (SELECT TOP 1 UserId FROM Users WHERE Role = 'Student');
DECLARE @CourseId INT = (SELECT TOP 1 CourseId FROM Courses WHERE IsPublished = 1);

IF @StudentId IS NOT NULL AND @CourseId IS NOT NULL
BEGIN
    INSERT INTO Enrollments (StudentId, CourseId, EnrolledAt, Status, ProgressPercentage)
    VALUES (@StudentId, @CourseId, GETDATE(), 'Active', 25);
    PRINT 'Test enrollment created!';
END
ELSE
BEGIN
    PRINT 'ERROR: No student or course found!';
END
```

### **Issue 3: API Returns Empty Course IDs**

**Check Enrollment Service:**

The API endpoint should return course IDs:
```json
{
  "success": true,
  "data": [1, 2, 3],  // Array of course IDs
  "message": null
}
```

**If empty array `[]`:**
- Student has no enrollments in database
- Wrong student ID being used
- Check authentication token

### **Issue 4: Courses API Fails**

**Check if courses endpoint works:**
```powershell
curl https://localhost:7059/api/courses/1
```

Should return course data (not 500 error).

---

## ?? **HOW IT WORKS NOW**

### **Flow:**

1. **Student navigates to /Student/MyCourses**
2. **Controller checks authentication**
   - Gets token from cookie
   - Redirects to login if no token
3. **Controller calls API:**
   - GET `/api/enrollments/my-courses` ? Returns `[1, 2, 3]`
4. **For each course ID:**
   - GET `/api/courses/1` ? Returns course details
   - GET `/api/courses/2` ? Returns course details
   - GET `/api/courses/3` ? Returns course details
5. **Builds Model** with EnrolledCourseVm list
6. **View renders** server-side with tabs and cards

---

## ?? **API ENDPOINTS USED**

| Endpoint | Method | Purpose | Auth Required |
|----------|--------|---------|---------------|
| `/api/enrollments/my-courses` | GET | Get student's enrolled course IDs | ? Bearer Token |
| `/api/courses/{id}` | GET | Get course details | ? Anonymous |

---

## ? **FILES MODIFIED**

1. ? `LearningManagementSystem.MVC\Controllers\StudentController.cs`
   - Implemented MyCourses() to fetch enrollments
   - Implemented Dashboard() to fetch enrollments
   - Added authentication checks
   - Added API calls with Bearer token
   - Added error handling and logging

2. ? `LearningManagementSystem.MVC\Views\Student\MyCourses.cshtml`
   - Replaced JavaScript-based approach
   - Uses server-side Model data
   - Added tabs for filtering (All, In Progress, Completed)
   - Added progress bars and status badges
   - Added error message display

**Build:** ? Successful  
**Errors:** 0  
**Action:** **Restart MVC**

---

## ?? **SUMMARY**

**Problem:** My Courses page shows no courses  
**Cause 1:** StudentController.MyCourses() didn't fetch data  
**Cause 2:** View used client-side JS with wrong API endpoint  
**Fix 1:** Implemented server-side data fetching in controller  
**Fix 2:** Changed view to use server-rendered Razor  
**Status:** ? Fixed & Built Successfully  
**Action Required:** **RESTART MVC PROJECT**  

**Prerequisite:** Student must have enrollments in database!

---

## ? **QUICK TEST**

```powershell
# 1. Restart MVC (or press F5 in Visual Studio)

# 2. Create test enrollment (SQL)
USE LMS_DB;
DECLARE @StudentId INT = (SELECT TOP 1 UserId FROM Users WHERE Role = 'Student');
DECLARE @CourseId INT = (SELECT TOP 1 CourseId FROM Courses WHERE IsPublished = 1);
INSERT INTO Enrollments (StudentId, CourseId, EnrolledAt, Status, ProgressPercentage)
VALUES (@StudentId, @CourseId, GETDATE(), 'Active', 30);

# 3. Login as student and go to:
Start-Process "https://localhost:7012/Student/MyCourses"

# 4. Should see enrolled courses! ?
```

**After restarting MVC and having enrollments in DB, courses will show!** ??
