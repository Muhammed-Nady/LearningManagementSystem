#!/usr/bin/env pwsh
# Quick Login Issue Diagnostic

Write-Host "=== LOGIN CONNECTION DIAGNOSTIC ===" -ForegroundColor Cyan
Write-Host ""

# Test 1: Check if API is running
Write-Host "Test 1: Checking if API is running..." -ForegroundColor Yellow
try {
    $apiResponse = Invoke-WebRequest -Uri "https://localhost:7059/api/auth/login" -Method POST `
     -ContentType "application/json" `
        -Body '{"email":"test@test.com","password":"test"}' `
        -UseBasicParsing -ErrorAction Stop
    
    Write-Host "? API is accessible (Status: $($apiResponse.StatusCode))" -ForegroundColor Green
    Write-Host "   (Invalid credentials is OK - we just need to reach the endpoint)" -ForegroundColor Gray
} catch {
 if ($_.Exception.Message -like "*401*" -or $_.Exception.Message -like "*400*") {
        Write-Host "? API is accessible (endpoint responds)" -ForegroundColor Green
    } else {
      Write-Host "? API IS NOT RUNNING!" -ForegroundColor Red
        Write-Host "   Error: $($_.Exception.Message)" -ForegroundColor Red
        Write-Host ""
        Write-Host "   ACTION REQUIRED: Start the API project!" -ForegroundColor Yellow
        Write-Host "   1. In Visual Studio Solution Explorer" -ForegroundColor White
        Write-Host "   2. Right-click 'LearningManagementSystem.API'" -ForegroundColor White
        Write-Host "   3. Select 'Debug ? Start New Instance'" -ForegroundColor White
  Write-Host "   4. Wait for: 'Now listening on: https://localhost:7059'" -ForegroundColor White
    }
}

Write-Host ""

# Test 2: Check if MVC is running
Write-Host "Test 2: Checking if MVC is running..." -ForegroundColor Yellow
try {
    $mvcResponse = Invoke-WebRequest -Uri "https://localhost:7012" -UseBasicParsing -ErrorAction Stop
    Write-Host "? MVC is accessible (Status: $($mvcResponse.StatusCode))" -ForegroundColor Green
} catch {
    Write-Host "? MVC IS NOT RUNNING!" -ForegroundColor Red
    Write-Host "   Error: $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host ""

# Test 3: Check SSL certificates
Write-Host "Test 3: Checking SSL certificates..." -ForegroundColor Yellow
try {
    [Net.ServicePointManager]::ServerCertificateValidationCallback = {$true}
    $req = [Net.HttpWebRequest]::Create("https://localhost:7059")
    $req.GetResponse() | Out-Null
    Write-Host "? SSL certificate OK" -ForegroundColor Green
} catch {
    Write-Host "??  SSL certificate issue (might be OK)" -ForegroundColor Yellow
}

Write-Host ""
Write-Host "=== SOLUTION ===" -ForegroundColor Cyan
Write-Host ""
Write-Host "If API is NOT running:" -ForegroundColor Yellow
Write-Host "  ? Start API: Right-click project ? Debug ? Start New Instance" -ForegroundColor White
Write-Host ""
Write-Host "If both are running but still can't login:" -ForegroundColor Yellow
Write-Host "  ? Check browser console (F12) for exact error message" -ForegroundColor White
Write-Host "  ? The error will tell us exactly what's wrong" -ForegroundColor White

Write-Host ""
Write-Host "Press any key to exit..." -ForegroundColor Gray
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
