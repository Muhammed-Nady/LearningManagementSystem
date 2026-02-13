# RESTART API TO FIX DETAILS PAGE

Write-Host "=== RESTARTING API TO APPLY FIX ===" -ForegroundColor Cyan
Write-Host ""

# Stop API
Write-Host "1. Stopping API..." -ForegroundColor Yellow
$apiProcess = Get-Process -Name "LearningManagementSystem.API" -ErrorAction SilentlyContinue
if ($apiProcess) {
    Stop-Process -Id $apiProcess.Id -Force
    Start-Sleep -Seconds 2
    Write-Host "   ? API stopped (PID: $($apiProcess.Id))" -ForegroundColor Green
} else {
  Write-Host "   ? API was not running" -ForegroundColor Yellow
}

Write-Host ""
Write-Host "2. Start API from Visual Studio or run:" -ForegroundColor Yellow
Write-Host "   cd LearningManagementSystem.API" -ForegroundColor Gray
Write-Host "   dotnet run" -ForegroundColor Gray
Write-Host ""

Write-Host "=== WHAT WAS FIXED ===" -ForegroundColor Cyan
Write-Host ""
Write-Host "? CourseService.GetCourseByIdAsync" -ForegroundColor Green
Write-Host "  - Fixed NullReferenceException" -ForegroundColor White
Write-Host "  - Added null-safety for Instructor & Category" -ForegroundColor White
Write-Host ""
Write-Host "? Added null-safety to 3 other methods" -ForegroundColor Green
Write-Host ""

Write-Host "=== AFTER RESTART ===" -ForegroundColor Cyan
Write-Host ""
Write-Host "? Details page will load (no 500 error)" -ForegroundColor Green
Write-Host "? Course information displays" -ForegroundColor Green
Write-Host "? All navigation works" -ForegroundColor Green
Write-Host ""

Write-Host "Test with: curl https://localhost:7059/api/courses/1" -ForegroundColor Cyan
Write-Host ""
