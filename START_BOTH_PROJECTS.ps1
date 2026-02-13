# Quick Start Script
# This starts both API and MVC projects

Write-Host "=== Starting LMS Projects ===" -ForegroundColor Cyan
Write-Host ""

# Check if projects exist
if (!(Test-Path "LearningManagementSystem.API\LearningManagementSystem.API.csproj")) {
    Write-Host "? API project not found!" -ForegroundColor Red
    exit
}

if (!(Test-Path "LearningManagementSystem.MVC\LearningManagementSystem.MVC.csproj")) {
    Write-Host "? MVC project not found!" -ForegroundColor Red
    exit
}

Write-Host "Starting API project..." -ForegroundColor Yellow
$apiJob = Start-Job -ScriptBlock {
    Set-Location $using:PWD
    cd LearningManagementSystem.API
    dotnet run
}

Write-Host "Waiting for API to start (10 seconds)..." -ForegroundColor Yellow
Start-Sleep -Seconds 10

Write-Host ""
Write-Host "Starting MVC project..." -ForegroundColor Yellow
$mvcJob = Start-Job -ScriptBlock {
    Set-Location $using:PWD
    cd LearningManagementSystem.MVC
    dotnet run
}

Write-Host ""
Write-Host "? Both projects starting..." -ForegroundColor Green
Write-Host ""
Write-Host "API should be at: https://localhost:7059" -ForegroundColor Cyan
Write-Host "MVC should be at: https://localhost:7012" -ForegroundColor Cyan
Write-Host ""
Write-Host "Press Ctrl+C to stop both projects" -ForegroundColor Yellow
Write-Host ""

# Wait for both jobs
Wait-Job $apiJob, $mvcJob
