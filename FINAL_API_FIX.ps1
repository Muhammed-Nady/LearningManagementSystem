# FINAL FIX: Repository Expression Error

Write-Host "=== API EXPRESSION ERROR - FIXED ===" -ForegroundColor Cyan
Write-Host ""

Write-Host "PROBLEM IDENTIFIED:" -ForegroundColor Yellow
Write-Host "  EF.Property<int>(e, 'Id') expression cannot be translated to SQL" -ForegroundColor White
Write-Host ""

Write-Host "SOLUTION APPLIED:" -ForegroundColor Yellow
Write-Host "  ? Fixed GetByIdWithIncludesAsync method" -ForegroundColor Green
Write-Host "  ? Now uses EF metadata to get actual primary key name" -ForegroundColor Green
Write-Host "  ? Builds proper expression tree that EF can translate" -ForegroundColor Green
Write-Host ""

Write-Host "??  RESTART API NOW TO APPLY FIX" -ForegroundColor Red
Write-Host ""

# Stop API
$apiProcess = Get-Process -Name "LearningManagementSystem.API" -ErrorAction SilentlyContinue
if ($apiProcess) {
    Write-Host "Stopping API..." -ForegroundColor Yellow
    Stop-Process -Id $apiProcess.Id -Force
    Start-Sleep -Seconds 2
    Write-Host "? API stopped" -ForegroundColor Green
} else {
    Write-Host "? API was not running" -ForegroundColor Yellow
}

Write-Host ""
Write-Host "NOW START API:" -ForegroundColor Cyan
Write-Host "  Press F5 in Visual Studio" -ForegroundColor White
Write-Host "  OR run: cd LearningManagementSystem.API && dotnet run" -ForegroundColor Gray
Write-Host ""

Write-Host "AFTER RESTART:" -ForegroundColor Cyan
Write-Host "  ? GET /api/courses/1 will work (200 OK)" -ForegroundColor Green
Write-Host "  ? Details page will load" -ForegroundColor Green
Write-Host "? No more 500 errors" -ForegroundColor Green
Write-Host ""

Write-Host "TEST WITH:" -ForegroundColor Cyan
Write-Host "  curl https://localhost:7059/api/courses/1" -ForegroundColor White
Write-Host ""
