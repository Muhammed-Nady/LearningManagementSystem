# Simple API Test
# This tests if the API is actually returning data

Write-Host "Testing API Connection..." -ForegroundColor Cyan
Write-Host ""

# Test 1: Check if API process is running
$apiProcess = Get-Process -Name "LearningManagementSystem.API" -ErrorAction SilentlyContinue
if ($apiProcess) {
    Write-Host "? API process is running (PID: $($apiProcess.Id))" -ForegroundColor Green
} else {
    Write-Host "? API is NOT running!" -ForegroundColor Red
    Write-Host "  -> Start the API project in Visual Studio" -ForegroundColor Yellow
    exit
}

Write-Host ""

# Test 2: Check if port 7059 is listening
Write-Host "Checking if API is listening on port 7059..." -ForegroundColor Yellow
$listening = netstat -ano | Select-String "7059.*LISTENING"
if ($listening) {
    Write-Host "? Port 7059 is listening" -ForegroundColor Green
} else {
    Write-Host "? Port 7059 is NOT listening!" -ForegroundColor Red
    Write-Host "  -> API might not have started correctly" -ForegroundColor Yellow
    exit
}

Write-Host ""

# Test 3: Try to connect using .NET WebClient
Write-Host "Attempting to fetch courses from API..." -ForegroundColor Yellow
try {
    # Use .NET WebClient which works with older PowerShell
    [System.Net.ServicePointManager]::ServerCertificateValidationCallback = {$true}
    $webClient = New-Object System.Net.WebClient
    $webClient.Headers.Add("User-Agent", "PowerShell")
    
    $response = $webClient.DownloadString("https://localhost:7059/api/courses")
    $data = $response | ConvertFrom-Json
 
    if ($data.success -and $data.data) {
        $courseCount = $data.data.Count
        Write-Host "? SUCCESS! API returned $courseCount courses" -ForegroundColor Green
   Write-Host ""
        Write-Host "Sample courses:" -ForegroundColor Cyan
        $data.data | Select-Object -First 3 | ForEach-Object {
         Write-Host "  - $($_.title) ($($_.price))" -ForegroundColor White
    }
      Write-Host ""
        Write-Host "=== API is working correctly! ===" -ForegroundColor Green
        Write-Host "The problem is likely in the MVC app or browser." -ForegroundColor Yellow
    } else {
        Write-Host "? API returned data but it's empty or invalid" -ForegroundColor Red
        Write-Host "Response: $response" -ForegroundColor DarkGray
  }
} catch {
    Write-Host "? FAILED to connect to API!" -ForegroundColor Red
    Write-Host "Error: $($_.Exception.Message)" -ForegroundColor DarkRed
    Write-Host ""
    Write-Host "Possible causes:" -ForegroundColor Yellow
    Write-Host "  1. API is not running on port 7059" -ForegroundColor White
    Write-Host "  2. Firewall is blocking the connection" -ForegroundColor White
    Write-Host "  3. API failed to start (check Output window)" -ForegroundColor White
    Write-Host ""
    Write-Host "Check Visual Studio Output window:" -ForegroundColor Cyan
    Write-Host "  View > Output > Show output from: LearningManagementSystem.API" -ForegroundColor Gray
}

Write-Host ""
Write-Host "=== Next Steps ===" -ForegroundColor Cyan
Write-Host ""
Write-Host "If API test succeeded:" -ForegroundColor Yellow
Write-Host "  1. Open browser DevTools (F12)" -ForegroundColor White
Write-Host "  2. Go to Console tab" -ForegroundColor White
Write-Host "  3. Refresh homepage (Ctrl+F5)" -ForegroundColor White
Write-Host "  4. Look for errors in red" -ForegroundColor White
Write-Host ""
Write-Host "  Then check Network tab:" -ForegroundColor White
Write-Host "  5. Look for request to '/courses'" -ForegroundColor White
Write-Host "  6. Check if it's Status 200 or error" -ForegroundColor White
Write-Host "  7. Click on it and view Response" -ForegroundColor White
