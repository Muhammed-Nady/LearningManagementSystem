#!/usr/bin/env pwsh
# Diagnose Courses Not Showing

Write-Host "=== COURSES DIAGNOSTIC ===" -ForegroundColor Cyan
Write-Host ""

# Test 1: Check if API is accessible
Write-Host "Test 1: Testing API endpoint..." -ForegroundColor Yellow
try {
    $response = Invoke-RestMethod -Uri "https://localhost:7059/api/courses" -UseBasicParsing -ErrorAction Stop
    Write-Host "? API is accessible" -ForegroundColor Green
    
    if ($response.success) {
        $courseCount = if ($response.data) { $response.data.Count } else { 0 }
        Write-Host "? API returned $courseCount courses" -ForegroundColor Green
        
        if ($courseCount -eq 0) {
            Write-Host "??  WARNING: No courses in database!" -ForegroundColor Red
            Write-Host "   Solution: Create and publish courses as instructor" -ForegroundColor Yellow
        } else {
          Write-Host "   Published courses found:" -ForegroundColor Cyan
   foreach ($course in $response.data | Select-Object -First 5) {
    $status = if ($course.isPublished) { "Published" } else { "Draft" }
   Write-Host "   - $($course.title) [$status]" -ForegroundColor Gray
     }
        }
    } else {
        Write-Host "? API returned error: $($response.message)" -ForegroundColor Red
    }
} catch {
    Write-Host "? Cannot connect to API!" -ForegroundColor Red
    Write-Host "   Error: $($_.Exception.Message)" -ForegroundColor Red
    Write-Host "   Solution: Start API project (LearningManagementSystem.API)" -ForegroundColor Yellow
}

Write-Host ""

# Test 2: Check if MVC is accessible
Write-Host "Test 2: Testing MVC application..." -ForegroundColor Yellow
try {
    $response = Invoke-WebRequest -Uri "https://localhost:7012" -UseBasicParsing -ErrorAction Stop
    Write-Host "? MVC is accessible (Status: $($response.StatusCode))" -ForegroundColor Green
} catch {
    Write-Host "? Cannot connect to MVC!" -ForegroundColor Red
    Write-Host "   Error: $($_.Exception.Message)" -ForegroundColor Red
    Write-Host "   Solution: Start MVC project (LearningManagementSystem.MVC)" -ForegroundColor Yellow
}

Write-Host ""

# Test 3: Check database
Write-Host "Test 3: Checking database..." -ForegroundColor Yellow
try {
    $query = "SELECT COUNT(*) as Total, SUM(CASE WHEN IsPublished = 1 THEN 1 ELSE 0 END) as Published FROM Courses"
    $result = Invoke-Sqlcmd -Query $query -ServerInstance "localhost" -Database "LMS_DB" -TrustServerCertificate
    
    $total = $result.Total
$published = $result.Published
 
    Write-Host "? Database accessible" -ForegroundColor Green
    Write-Host "   Total courses: $total" -ForegroundColor Cyan
  Write-Host "   Published courses: $published" -ForegroundColor Cyan
    
    if ($published -eq 0) {
 Write-Host "??  No published courses in database!" -ForegroundColor Red
        Write-Host "   Action: Publish courses from Instructor dashboard" -ForegroundColor Yellow
    }
} catch {
    Write-Host "??  Cannot query database (optional check)" -ForegroundColor Yellow
    Write-Host "   Error: $($_.Exception.Message)" -ForegroundColor Gray
}

Write-Host ""
Write-Host "=== SUMMARY ===" -ForegroundColor Cyan
Write-Host ""

Write-Host "Next steps to debug:" -ForegroundColor Yellow
Write-Host "1. Open browser: https://localhost:7012" -ForegroundColor White
Write-Host "2. Press F12 to open Developer Tools" -ForegroundColor White
Write-Host "3. Go to 'Console' tab" -ForegroundColor White
Write-Host "4. Look for red errors" -ForegroundColor White
Write-Host "5. Go to 'Network' tab" -ForegroundColor White
Write-Host "6. Refresh page (F5)" -ForegroundColor White
Write-Host "7. Look for '/api/courses' request" -ForegroundColor White
Write-Host "8. Check the response" -ForegroundColor White

Write-Host ""
Write-Host "Press any key to exit..." -ForegroundColor Gray
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
