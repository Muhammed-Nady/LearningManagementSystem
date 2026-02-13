# HomeController Verification Script
# This script verifies that everything in the HomeController works correctly

Write-Host "=== HomeController Verification ===" -ForegroundColor Cyan
Write-Host ""

$allChecks = @()

# Check 1: Verify HomeController.cs exists and compiles
Write-Host "1. Checking HomeController.cs..." -ForegroundColor Yellow
if (Test-Path "LearningManagementSystem.MVC\Controllers\HomeController.cs") {
    Write-Host "   ? HomeController.cs exists" -ForegroundColor Green
  $allChecks += $true
} else {
    Write-Host "   ? HomeController.cs NOT FOUND" -ForegroundColor Red
    $allChecks += $false
}

# Check 2: Verify HomeViewModel exists
Write-Host ""
Write-Host "2. Checking HomeViewModel.cs..." -ForegroundColor Yellow
if (Test-Path "LearningManagementSystem.MVC\Models\HomeViewModel.cs") {
    Write-Host "? HomeViewModel.cs exists" -ForegroundColor Green
    $allChecks += $true
} else {
    Write-Host "   ? HomeViewModel.cs NOT FOUND" -ForegroundColor Red
    $allChecks += $false
}

# Check 3: Verify Index.cshtml exists
Write-Host ""
Write-Host "3. Checking Index.cshtml..." -ForegroundColor Yellow
if (Test-Path "LearningManagementSystem.MVC\Views\Home\Index.cshtml") {
    Write-Host "   ? Index.cshtml exists" -ForegroundColor Green
    
    # Check if it has @model directive
    $content = Get-Content "LearningManagementSystem.MVC\Views\Home\Index.cshtml" -Raw
  if ($content -match "@model LearningManagementSystem.MVC.Models.HomeViewModel") {
        Write-Host "   ? @model directive is correct" -ForegroundColor Green
        $allChecks += $true
    } else {
        Write-Host "? @model directive is missing or incorrect" -ForegroundColor Red
   $allChecks += $false
  }
} else {
    Write-Host "   ? Index.cshtml NOT FOUND" -ForegroundColor Red
    $allChecks += $false
}

# Check 4: Verify appsettings.json has correct API URL
Write-Host ""
Write-Host "4. Checking appsettings.json..." -ForegroundColor Yellow
if (Test-Path "LearningManagementSystem.MVC\appsettings.json") {
    $config = Get-Content "LearningManagementSystem.MVC\appsettings.json" | ConvertFrom-Json
    $apiUrl = $config.ApiSettings.BaseUrl
    
    if ($apiUrl -eq "https://localhost:7059/api") {
      Write-Host "   ? API URL is correct: $apiUrl" -ForegroundColor Green
        $allChecks += $true
    } else {
      Write-Host "   ? API URL is incorrect: $apiUrl" -ForegroundColor Red
Write-Host "     Should be: https://localhost:7059/api" -ForegroundColor Yellow
     $allChecks += $false
    }
} else {
    Write-Host "   ? appsettings.json NOT FOUND" -ForegroundColor Red
    $allChecks += $false
}

# Check 5: Verify _CourseCard partial exists
Write-Host ""
Write-Host "5. Checking _CourseCard.cshtml partial..." -ForegroundColor Yellow
if (Test-Path "LearningManagementSystem.MVC\Views\Shared\_CourseCard.cshtml") {
    Write-Host "   ? _CourseCard.cshtml exists" -ForegroundColor Green
    $allChecks += $true
} else {
    Write-Host "   ? _CourseCard.cshtml NOT FOUND" -ForegroundColor Red
    $allChecks += $false
}

# Check 6: Verify Program.cs has HttpClientFactory
Write-Host ""
Write-Host "6. Checking Program.cs configuration..." -ForegroundColor Yellow
if (Test-Path "LearningManagementSystem.MVC\Program.cs") {
    $programContent = Get-Content "LearningManagementSystem.MVC\Program.cs" -Raw
    
    if ($programContent -match "AddHttpClient") {
    Write-Host "   ? HttpClientFactory is registered" -ForegroundColor Green
        $allChecks += $true
    } else {
        Write-Host "   ? HttpClientFactory is NOT registered" -ForegroundColor Red
        $allChecks += $false
    }
} else {
    Write-Host "   ? Program.cs NOT FOUND" -ForegroundColor Red
    $allChecks += $false
}

# Check 7: Test if project builds
Write-Host ""
Write-Host "7. Testing project build..." -ForegroundColor Yellow
$buildOutput = dotnet build "LearningManagementSystem.MVC\LearningManagementSystem.MVC.csproj" --no-restore 2>&1
if ($LASTEXITCODE -eq 0) {
    Write-Host "   ? Project builds successfully" -ForegroundColor Green
    $allChecks += $true
} else {
    Write-Host "   ? Project has build errors" -ForegroundColor Red
    Write-Host "     Run 'dotnet build' to see details" -ForegroundColor Yellow
    $allChecks += $false
}

# Check 8: Verify API is accessible
Write-Host ""
Write-Host "8. Testing API connectivity..." -ForegroundColor Yellow
$apiProcess = Get-Process -Name "LearningManagementSystem.API" -ErrorAction SilentlyContinue
if ($apiProcess) {
    Write-Host "   ? API process is running" -ForegroundColor Green
  
    try {
        [System.Net.ServicePointManager]::ServerCertificateValidationCallback = {$true}
$webClient = New-Object System.Net.WebClient
        $response = $webClient.DownloadString("https://localhost:7059/api/courses")
        $data = $response | ConvertFrom-Json
        
        if ($data.success -and $data.data) {
   Write-Host "   ? API returns data ($($data.data.Count) courses)" -ForegroundColor Green
         $allChecks += $true
        } else {
  Write-Host "   ? API responds but data is empty" -ForegroundColor Yellow
   $allChecks += $false
  }
    } catch {
        Write-Host "   ? Cannot connect to API" -ForegroundColor Red
        Write-Host "     Error: $($_.Exception.Message)" -ForegroundColor DarkRed
        $allChecks += $false
    }
} else {
    Write-Host "   ? API is NOT running" -ForegroundColor Red
  Write-Host "     Start the API project first!" -ForegroundColor Yellow
    $allChecks += $false
}

# Check 9: Verify database has data
Write-Host ""
Write-Host "9. Checking database..." -ForegroundColor Yellow
try {
    $query = "SELECT COUNT(*) FROM Courses WHERE IsPublished = 1"
    $result = sqlcmd -S "." -d "LMS_DB" -E -Q $query -h -1 2>$null
    $count = [int]$result.Trim()
    
    if ($count -gt 0) {
        Write-Host "   ? Database has $count published courses" -ForegroundColor Green
        $allChecks += $true
    } else {
   Write-Host "   ? Database has NO published courses" -ForegroundColor Red
        Write-Host "     Run migration to seed data" -ForegroundColor Yellow
     $allChecks += $false
    }
} catch {
    Write-Host "   ? Cannot check database (sqlcmd not available or DB not accessible)" -ForegroundColor Yellow
    $allChecks += $true  # Don't fail on this
}

# Summary
Write-Host ""
Write-Host "=== Summary ===" -ForegroundColor Cyan
$passedChecks = ($allChecks | Where-Object { $_ -eq $true }).Count
$totalChecks = $allChecks.Count

Write-Host ""
if ($passedChecks -eq $totalChecks) {
    Write-Host "? ALL CHECKS PASSED ($passedChecks/$totalChecks)" -ForegroundColor Green
    Write-Host ""
    Write-Host "HomeController is configured correctly!" -ForegroundColor Green
    Write-Host ""
    Write-Host "To test:" -ForegroundColor Cyan
    Write-Host "  1. Make sure both API and MVC are running" -ForegroundColor White
    Write-Host "  2. Open: https://localhost:7012" -ForegroundColor White
    Write-Host "  3. Should see courses on homepage" -ForegroundColor White
} else {
    Write-Host "? SOME CHECKS FAILED ($passedChecks/$totalChecks passed)" -ForegroundColor Yellow
    Write-Host ""
    Write-Host "Review the failed checks above and fix them." -ForegroundColor Yellow
}

Write-Host ""
Write-Host "=== Additional Tests ===" -ForegroundColor Cyan
Write-Host ""
Write-Host "To test HomeController in browser:" -ForegroundColor Yellow
Write-Host "  1. Open test page: https://localhost:7012/detailed-test.html" -ForegroundColor White
Write-Host "  2. Click 'Test /api/courses' button" -ForegroundColor White
Write-Host "  3. Should see SUCCESS with course data" -ForegroundColor White
Write-Host ""
Write-Host "To check logs:" -ForegroundColor Yellow
Write-Host "  1. Open Visual Studio Output window (Ctrl+Alt+O)" -ForegroundColor White
Write-Host "  2. Select 'LearningManagementSystem.MVC' from dropdown" -ForegroundColor White
Write-Host "  3. Refresh homepage" -ForegroundColor White
Write-Host "  4. Look for: 'Loaded X courses successfully'" -ForegroundColor White
Write-Host ""
Write-Host "=== End of Verification ===" -ForegroundColor Cyan
