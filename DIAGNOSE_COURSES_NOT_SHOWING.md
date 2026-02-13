# ?? DIAGNOSE: Courses Not Showing

## Quick Diagnostic Steps

### Step 1: Check if API is Running
Open browser and navigate to:
```
https://localhost:7059/api/courses
```

**Expected:** JSON response with courses list  
**If Error:** API is not running or has issues

### Step 2: Check Browser Console
```
1. Open your homepage: https://localhost:7012
2. Press F12 (Developer Tools)
3. Go to "Console" tab
4. Look for red errors
```

**Common errors to look for:**
- "Failed to fetch"
- "NetworkError"
- "Unexpected token"
- JavaScript errors in HomeController

### Step 3: Check Network Tab
```
1. Keep F12 open
2. Go to "Network" tab
3. Refresh page (F5)
4. Look for request to: /api/courses
```

**What to check:**
- Status: Should be 200 OK
- Response: Should have JSON with courses
- If 404/500: API endpoint issue
- If no request: JavaScript not running

### Step 4: Check MVC Logs
```
View ? Output ? Show output from: LearningManagementSystem.MVC
```

**Look for:**
- "Fetching data from API"
- "Loaded X courses successfully"
- Any error messages

### Step 5: Check API Logs
```
View ? Output ? Show output from: LearningManagementSystem.API
```

**Look for:**
- GET requests to /api/courses
- Any 500 errors
- Connection issues

---

## Run This PowerShell Script

Save as `diagnose-courses.ps1` and run:

```powershell
# Diagnose Courses Not Showing

Write-Host "=== COURSES DIAGNOSTIC ===" -ForegroundColor Cyan
Write-Host ""

# Test 1: Check if API is accessible
Write-Host "Test 1: Testing API..." -ForegroundColor Yellow
try {
    $response = Invoke-RestMethod -Uri "https://localhost:7059/api/courses" -UseBasicParsing
    Write-Host "? API is accessible" -ForegroundColor Green
    
    if ($response.success) {
  $courseCount = $response.data.Count
        Write-Host "? API returned $courseCount courses" -ForegroundColor Green
        
      if ($courseCount -eq 0) {
            Write-Host "??  WARNING: No courses in database!" -ForegroundColor Red
     Write-Host "   Solution: Add courses as instructor or run seeding script" -ForegroundColor Yellow
        } else {
   Write-Host "   Courses found:" -ForegroundColor Cyan
      foreach ($course in $response.data) {
       Write-Host "   - $($course.title) (Published: $($course.isPublished))" -ForegroundColor Gray
   }
        }
    } else {
        Write-Host "? API returned error: $($response.message)" -ForegroundColor Red
    }
} catch {
    Write-Host "? Cannot connect to API!" -ForegroundColor Red
    Write-Host "   Error: $($_.Exception.Message)" -ForegroundColor Red
    Write-Host "   Solution: Make sure API project is running" -ForegroundColor Yellow
}

Write-Host ""

# Test 2: Check if MVC is accessible
Write-Host "Test 2: Testing MVC..." -ForegroundColor Yellow
try {
    $response = Invoke-WebRequest -Uri "https://localhost:7012" -UseBasicParsing
    Write-Host "? MVC is accessible (Status: $($response.StatusCode))" -ForegroundColor Green
} catch {
    Write-Host "? Cannot connect to MVC!" -ForegroundColor Red
    Write-Host "   Error: $($_.Exception.Message)" -ForegroundColor Red
    Write-Host "   Solution: Make sure MVC project is running" -ForegroundColor Yellow
}

Write-Host ""
Write-Host "=== NEXT STEPS ===" -ForegroundColor Cyan

# Check if no courses
if ($courseCount -eq 0) {
    Write-Host "1. Login as Instructor" -ForegroundColor Yellow
    Write-Host "2. Create and publish a course" -ForegroundColor Yellow
    Write-Host "3. Refresh homepage" -ForegroundColor Yellow
} else {
    Write-Host "1. Open browser Developer Tools (F12)" -ForegroundColor Yellow
    Write-Host "2. Go to Console tab" -ForegroundColor Yellow
    Write-Host "3. Look for JavaScript errors" -ForegroundColor Yellow
    Write-Host "4. Go to Network tab" -ForegroundColor Yellow
    Write-Host "5. Refresh page and check if /api/courses is called" -ForegroundColor Yellow
}

Write-Host ""
Write-Host "Press any key to exit..." -ForegroundColor Gray
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
```

---

## Common Issues & Solutions

### Issue 1: API Not Running
**Symptom:** Cannot access https://localhost:7059/api/courses  
**Solution:**
```
1. In Visual Studio, right-click LearningManagementSystem.API
2. Debug ? Start New Instance
3. Wait for "Now listening on: https://localhost:7059"
```

### Issue 2: No Courses in Database
**Symptom:** API returns empty array `[]`  
**Solution:**
```
1. Login as Instructor
2. Go to "My Courses"
3. Click "Create New Course"
4. Fill in details and create
5. Click "Publish"
6. Refresh homepage
```

### Issue 3: Courses Not Published
**Symptom:** Courses exist but don't show
**Check:**
```sql
SELECT CourseId, Title, IsPublished FROM Courses;
```
**Solution:** Publish courses from Instructor dashboard

### Issue 4: JavaScript Error
**Symptom:** Console shows errors  
**Solution:**
```
1. Check if auth.js is loaded
2. Check if fetch() is working
3. Hard refresh (Ctrl+Shift+R)
```

### Issue 5: CORS Error
**Symptom:** "CORS policy" error in console  
**Solution:** Already configured, but verify API Program.cs has:
```csharp
builder.Services.AddCors(options => {
    options.AddPolicy("AllowAll", policy => {
        policy.AllowAnyOrigin()
    .AllowAnyMethod()
  .AllowAnyHeader();
    });
});
```

---

## Tell Me What You See

Please check and tell me:

1. **API Test:** Can you open `https://localhost:7059/api/courses` in browser?
   - What do you see? (JSON, error, nothing?)

2. **Browser Console:** Any red errors in F12 Console?

3. **Homepage:** What exactly shows on homepage?
   - "No courses available yet" message?
   - Blank space where courses should be?
   - Loading spinner that never stops?
   - Error message?

4. **Both Projects Running?**
   - API: https://localhost:7059
   - MVC: https://localhost:7012

Share what you see and I'll provide the exact fix! ??
