# Quick Diagnosis Script
# Run this to check why courses aren't showing

Write-Host "=== LMS Troubleshooting ===" -ForegroundColor Cyan
Write-Host ""

# 1. Check if processes are running
Write-Host "1. Checking Running Processes..." -ForegroundColor Yellow
$apiProcess = Get-Process -Name "LearningManagementSystem.API" -ErrorAction SilentlyContinue
$mvcProcess = Get-Process -Name "LearningManagementSystem.MVC" -ErrorAction SilentlyContinue

if ($apiProcess) {
    Write-Host "   ? API is running (PID: $($apiProcess.Id))" -ForegroundColor Green
} else {
    Write-Host "   ? API is NOT running" -ForegroundColor Red
    Write-Host "   -> Start the API project first!" -ForegroundColor Yellow
}

if ($mvcProcess) {
    Write-Host "   ? MVC is running (PID: $($mvcProcess.Id))" -ForegroundColor Green
} else {
    Write-Host "   ? MVC is NOT running" -ForegroundColor Red
    Write-Host "   -> Start the MVC project!" -ForegroundColor Yellow
}

Write-Host ""

# 2. Check API connectivity
Write-Host "2. Testing API Endpoint..." -ForegroundColor Yellow
try {
    $response = Invoke-WebRequest -Uri "https://localhost:7059/api/courses" -Method GET -SkipCertificateCheck -TimeoutSec 5 2>$null
    $data = ($response.Content | ConvertFrom-Json)
    
  if ($data.success -and $data.data) {
        $courseCount = $data.data.Count
        Write-Host "   ? API returned $courseCount courses" -ForegroundColor Green
        
        if ($courseCount -eq 0) {
         Write-Host "   ? Database has NO published courses!" -ForegroundColor Red
        } else {
      Write-Host "   Sample course: $($data.data[0].title)" -ForegroundColor Cyan
        }
    } else {
 Write-Host "   ? API returned empty or invalid response" -ForegroundColor Red
    }
} catch {
    Write-Host "   ? Cannot connect to API at https://localhost:7059" -ForegroundColor Red
    Write-Host "   Error: $($_.Exception.Message)" -ForegroundColor DarkRed
Write-Host "   -> Make sure API is running and listening on port 7059" -ForegroundColor Yellow
}

Write-Host ""

# 3. Check Database
Write-Host "3. Checking Database..." -ForegroundColor Yellow
try {
    $query = "SELECT COUNT(*) as Total FROM Courses WHERE IsPublished = 1"
    $result = Invoke-Sqlcmd -ServerInstance "." -Database "LMS_DB" -Query $query -TrustServerCertificate
    
    $count = $result.Total
    if ($count -gt 0) {
      Write-Host "   ? Database has $count published courses" -ForegroundColor Green
    } else {
        Write-Host "   ? Database has 0 published courses" -ForegroundColor Red
        Write-Host "   -> Run database seed migration!" -ForegroundColor Yellow
    }
} catch {
    Write-Host "   ? Cannot connect to database" -ForegroundColor Red
    Write-Host " Error: $($_.Exception.Message)" -ForegroundColor DarkRed
}

Write-Host ""

# 4. Check MVC Homepage
Write-Host "4. Testing MVC Homepage..." -ForegroundColor Yellow
try {
    $response = Invoke-WebRequest -Uri "https://localhost:7012/" -Method GET -SkipCertificateCheck -TimeoutSec 5 2>$null
    
    if ($response.StatusCode -eq 200) {
    Write-Host "   ? Homepage loads successfully" -ForegroundColor Green
        
        $content = $response.Content
        if ($content -match "No courses available yet") {
  Write-Host "   ? Homepage shows 'No courses available yet' message" -ForegroundColor Yellow
     Write-Host "   -> This means API call succeeded but returned no data" -ForegroundColor Yellow
        } elseif ($content -match "course-card|CourseId") {
     Write-Host "   ? Homepage appears to have course cards" -ForegroundColor Green
        } else {
    Write-Host "   ? Cannot determine if courses are showing" -ForegroundColor Yellow
        }
    }
} catch {
    Write-Host "   ? Cannot connect to MVC at https://localhost:7012" -ForegroundColor Red
    Write-Host "   Error: $($_.Exception.Message)" -ForegroundColor DarkRed
}

Write-Host ""

# 5. Check Configuration Files
Write-Host "5. Checking Configuration..." -ForegroundColor Yellow

$mvcConfig = Get-Content "LearningManagementSystem.MVC\appsettings.json" | ConvertFrom-Json
$apiUrl = $mvcConfig.ApiSettings.BaseUrl

if ($apiUrl -eq "https://localhost:7059/api") {
    Write-Host "   ? MVC points to correct API URL: $apiUrl" -ForegroundColor Green
} else {
    Write-Host "   ? MVC points to WRONG API URL: $apiUrl" -ForegroundColor Red
    Write-Host "   -> Should be: https://localhost:7059/api" -ForegroundColor Yellow
}

Write-Host ""

# Summary
Write-Host "=== Summary ===" -ForegroundColor Cyan
Write-Host ""
Write-Host "If courses aren't showing, most likely causes:" -ForegroundColor Yellow
Write-Host "  1. Database is empty (no published courses)" -ForegroundColor White
Write-Host "  2. API is not running or not accessible" -ForegroundColor White
Write-Host "  3. Wrong API URL in configuration" -ForegroundColor White
Write-Host "  4. CORS blocking the requests" -ForegroundColor White
Write-Host ""
Write-Host "Quick Fix Commands:" -ForegroundColor Cyan
Write-Host "  # Seed database:" -ForegroundColor White
Write-Host "  dotnet ef database drop --force --project LearningManagementSystem.Infrastructrue --startup-project LearningManagementSystem.API" -ForegroundColor Gray
Write-Host "  dotnet ef database update --project LearningManagementSystem.Infrastructrue --startup-project LearningManagementSystem.API" -ForegroundColor Gray
Write-Host ""
Write-Host "  # Restart projects:" -ForegroundColor White
Write-Host "  # Stop both, then start API first, then MVC" -ForegroundColor Gray
Write-Host ""
Write-Host "=== End of Diagnosis ===" -ForegroundColor Cyan
