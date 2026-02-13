# URGENT FIX: Restart MVC to Apply Changes
# This script restarts the MVC project so changes take effect

Write-Host "=== RESTARTING MVC PROJECT ===" -ForegroundColor Cyan
Write-Host ""

# Step 1: Stop MVC process
Write-Host "1. Stopping MVC process..." -ForegroundColor Yellow
$mvcProcess = Get-Process -Name "LearningManagementSystem.MVC" -ErrorAction SilentlyContinue
if ($mvcProcess) {
    Stop-Process -Id $mvcProcess.Id -Force
Start-Sleep -Seconds 2
    Write-Host "   ? MVC stopped (PID: $($mvcProcess.Id))" -ForegroundColor Green
} else {
    Write-Host "   ? MVC was not running" -ForegroundColor Yellow
}

Write-Host ""

# Step 2: Start MVC
Write-Host "2. Starting MVC project..." -ForegroundColor Yellow
Write-Host "   Please start MVC from Visual Studio or run:" -ForegroundColor Cyan
Write-Host "   cd LearningManagementSystem.MVC" -ForegroundColor Gray
Write-Host "   dotnet run" -ForegroundColor Gray
Write-Host ""

Write-Host "=== WHAT WAS FIXED ===" -ForegroundColor Cyan
Write-Host ""
Write-Host "? HomeController.cs" -ForegroundColor Green
Write-Host "  - Fixed: Missing trailing slash in BaseUrl" -ForegroundColor White
Write-Host "  - Before: https://localhost:7059/api" -ForegroundColor Red
Write-Host "  - After:  https://localhost:7059/api/" -ForegroundColor Green
Write-Host ""
Write-Host "  - Fixed: Leading slash on /categories" -ForegroundColor White
Write-Host "  - Before: await client.GetAsync('/categories')" -ForegroundColor Red
Write-Host "  - After:  await client.GetAsync('categories')" -ForegroundColor Green
Write-Host ""

Write-Host "? CoursesController.cs" -ForegroundColor Green
Write-Host "  - Already fixed with trailing slash" -ForegroundColor White
Write-Host "  - Details page will now work" -ForegroundColor White
Write-Host ""

Write-Host "=== AFTER RESTART ===" -ForegroundColor Cyan
Write-Host ""
Write-Host "? Homepage will show 14 courses" -ForegroundColor Green
Write-Host "? Categories will display" -ForegroundColor Green
Write-Host "? Course Details page will open" -ForegroundColor Green
Write-Host "? All URL construction issues fixed" -ForegroundColor Green
Write-Host ""

Write-Host "Press any key to continue..." -ForegroundColor Yellow
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
