# Diagnose IsSuccessStatusCode = False Issue

Write-Host "=== Diagnosing API Response Issue ===" -ForegroundColor Cyan
Write-Host ""

# Test 1: Check what status code the API is actually returning
Write-Host "1. Testing API /courses endpoint..." -ForegroundColor Yellow
try {
    [System.Net.ServicePointManager]::ServerCertificateValidationCallback = {$true}
    $webClient = New-Object System.Net.WebClient
    $webClient.Headers.Add("User-Agent", "PowerShell")
    
    try {
     $response = $webClient.DownloadString("https://localhost:7059/api/courses")
        $data = $response | ConvertFrom-Json
        
        Write-Host "   ? API returned data successfully" -ForegroundColor Green
        Write-Host "   Success: $($data.success)" -ForegroundColor Cyan
        Write-Host "   Courses: $($data.data.Count)" -ForegroundColor Cyan
    } catch {
        $statusCode = $_.Exception.Response.StatusCode.value__
        $statusDescription = $_.Exception.Response.StatusDescription
   
     Write-Host "   ? API returned error status" -ForegroundColor Red
        Write-Host "   Status Code: $statusCode" -ForegroundColor Yellow
        Write-Host "   Status: $statusDescription" -ForegroundColor Yellow
        Write-Host "   Error: $($_.Exception.Message)" -ForegroundColor DarkRed
        
      # Try to read error response
        try {
            $reader = New-Object System.IO.StreamReader($_.Exception.Response.GetResponseStream())
            $errorBody = $reader.ReadToEnd()
        Write-Host "   Error Body: $errorBody" -ForegroundColor DarkRed
        } catch {}
    }
} catch {
    Write-Host "   ? Cannot connect to API" -ForegroundColor Red
    Write-Host "   Error: $($_.Exception.Message)" -ForegroundColor DarkRed
}

Write-Host ""

# Test 2: Check API process and port
Write-Host "2. Checking API process..." -ForegroundColor Yellow
$apiProcess = Get-Process -Name "LearningManagementSystem.API" -ErrorAction SilentlyContinue
if ($apiProcess) {
    Write-Host "   ? API process is running (PID: $($apiProcess.Id))" -ForegroundColor Green
} else {
    Write-Host "   ? API is NOT running!" -ForegroundColor Red
    Write-Host "   -> Start the API project" -ForegroundColor Yellow
    exit
}

$listening = netstat -ano | Select-String "7059.*LISTENING"
if ($listening) {
  Write-Host "   ? Port 7059 is listening" -ForegroundColor Green
} else {
    Write-Host "   ? Port 7059 is NOT listening!" -ForegroundColor Red
}

Write-Host ""

# Test 3: Test with HttpClient (same as HomeController)
Write-Host "3. Testing with HttpClient (like HomeController)..." -ForegroundColor Yellow
Add-Type -AssemblyName System.Net.Http

$handler = New-Object System.Net.Http.HttpClientHandler
$handler.ServerCertificateCustomValidationCallback = { $true }

$httpClient = New-Object System.Net.Http.HttpClient($handler)
$httpClient.BaseAddress = [System.Uri]::new("https://localhost:7059/api")
$httpClient.Timeout = [System.TimeSpan]::FromSeconds(30)

try {
    $response = $httpClient.GetAsync("/courses").Result
    
    Write-Host "   Status Code: $($response.StatusCode) ($([int]$response.StatusCode))" -ForegroundColor Cyan
    Write-Host "   Is Success: $($response.IsSuccessStatusCode)" -ForegroundColor Cyan
    Write-Host "   Reason: $($response.ReasonPhrase)" -ForegroundColor Cyan
 
    if ($response.IsSuccessStatusCode) {
        $content = $response.Content.ReadAsStringAsync().Result
        $data = $content | ConvertFrom-Json
        Write-Host "   ? Success! Got $($data.data.Count) courses" -ForegroundColor Green
    } else {
        Write-Host "   ? Request failed!" -ForegroundColor Red
 $errorContent = $response.Content.ReadAsStringAsync().Result
      Write-Host "   Error Response:" -ForegroundColor Yellow
        Write-Host "   $errorContent" -ForegroundColor DarkRed
    }
} catch {
    Write-Host "   ? Exception occurred!" -ForegroundColor Red
    Write-Host "   Error: $($_.Exception.Message)" -ForegroundColor DarkRed
    
  if ($_.Exception.InnerException) {
        Write-Host "   Inner Error: $($_.Exception.InnerException.Message)" -ForegroundColor DarkRed
    }
}

$httpClient.Dispose()

Write-Host ""

# Test 4: Check API logs
Write-Host "4. Possible Causes:" -ForegroundColor Yellow
Write-Host ""
Write-Host "   If Status Code is 401 (Unauthorized):" -ForegroundColor White
Write-Host "   -> API requires authentication for /courses endpoint" -ForegroundColor Gray
Write-Host "     -> Check if [AllowAnonymous] attribute exists" -ForegroundColor Gray
Write-Host ""
Write-Host "   If Status Code is 404 (Not Found):" -ForegroundColor White
Write-Host "     -> API endpoint doesn't exist or routing is wrong" -ForegroundColor Gray
Write-Host "     -> Check API Controller route" -ForegroundColor Gray
Write-Host ""
Write-Host "   If Status Code is 500 (Internal Server Error):" -ForegroundColor White
Write-Host "     -> API has an exception" -ForegroundColor Gray
Write-Host "     -> Check API Output window for error details" -ForegroundColor Gray
Write-Host ""
Write-Host "   If Status Code is 0 or timeout:" -ForegroundColor White
Write-Host "   -> Cannot connect to API" -ForegroundColor Gray
Write-Host "     -> SSL/TLS error" -ForegroundColor Gray
Write-Host ""

Write-Host "=== Next Steps ===" -ForegroundColor Cyan
Write-Host ""
Write-Host "1. Check Visual Studio Output window:" -ForegroundColor Yellow
Write-Host "- View > Output (Ctrl+Alt+O)" -ForegroundColor White
Write-Host "   - Select 'LearningManagementSystem.API' from dropdown" -ForegroundColor White
Write-Host "   - Look for error messages when you refresh homepage" -ForegroundColor White
Write-Host ""
Write-Host "2. Check API Swagger:" -ForegroundColor Yellow
Write-Host "   - Open: https://localhost:7059/swagger" -ForegroundColor White
Write-Host "   - Try /api/courses endpoint manually" -ForegroundColor White
Write-Host "   - Check if it returns 200 OK" -ForegroundColor White
Write-Host ""
Write-Host "3. Check MVC Output window:" -ForegroundColor Yellow
Write-Host "   - Select 'LearningManagementSystem.MVC' from dropdown" -ForegroundColor White
Write-Host "   - Look for 'Courses API returned error' message" -ForegroundColor White
Write-Host "   - It will show the exact status code and error" -ForegroundColor White
Write-Host ""
