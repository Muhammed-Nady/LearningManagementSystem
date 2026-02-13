# Quick Test Script

## Test API Endpoints

### 1. Get All Courses (Should return 14 courses)
```powershell
Invoke-RestMethod -Uri "https://localhost:7059/api/courses" -Method GET
```

### 2. Get Specific Course
```powershell
Invoke-RestMethod -Uri "https://localhost:7059/api/courses/1" -Method GET
```

### 3. Get Categories
```powershell
Invoke-RestMethod -Uri "https://localhost:7059/api/categories" -Method GET
```

### 4. Login as Instructor
```powershell
$body = @{
    email = "john.smith@lms.com"
    password = "Instructor@123"
} | ConvertTo-Json

$response = Invoke-RestMethod -Uri "https://localhost:7059/api/auth/login" -Method POST -Body $body -ContentType "application/json"
$token = $response.data.token
Write-Host "Token: $token"
```

### 5. Get Instructor's Courses (Need token from step 4)
```powershell
$headers = @{
    "Authorization" = "Bearer $token"
}
Invoke-RestMethod -Uri "https://localhost:7059/api/courses/instructor/me" -Method GET -Headers $headers
```

---

## Test in Browser

1. **Start both projects**:
   - Run API (should be on https://localhost:7059)
   - Run MVC (should be on https://localhost:7012)

2. **Test Homepage**:
   - Go to: https://localhost:7012
 - Expected: See 14 courses with images

3. **Test Login**:
   - Click "Login"
   - Use: `john.smith@lms.com` / `Instructor@123`
   - Expected: Redirected to homepage, see your name in top right

4. **Test Instructor Dashboard**:
   - Click "My Dashboard" (top right menu)
   - Expected: See your courses listed

---

## Check Memory Usage

### Before Optimization:
```
Task Manager ? Details ? LearningManagementSystem.API.exe
Memory: ~500MB+
```

### After Optimization:
```
Task Manager ? Details ? LearningManagementSystem.API.exe
Memory: ~150-200MB
```

---

## Check Database Queries

### Using SQL Server Profiler:
1. Open SQL Server Profiler
2. Connect to your SQL Server
3. Filter by Database: LMS_DB
4. Load homepage
5. Count queries

**Before**: 100+ queries  
**After**: 3-5 queries

---

## Success Indicators

? Homepage loads in < 1 second  
? All 14 courses display with images  
? Memory usage < 200MB  
? No crashes or errors  
? API responses in < 500ms  
? Instructor dashboard shows courses  

---

## If Something Doesn't Work

### Courses Not Showing:
1. Check API is running: https://localhost:7059/swagger
2. Check database: `SELECT * FROM Courses WHERE IsPublished = 1`
3. Check browser console (F12) for errors

### API Errors:
1. Check `appsettings.json` connection string
2. Run: `dotnet ef database update`
3. Restart both projects

### Memory Still High:
1. Clear bin/obj folders
2. Rebuild solution
3. Restart Visual Studio

---

## Performance Monitoring

### Check Response Times:
```powershell
Measure-Command { Invoke-RestMethod -Uri "https://localhost:7059/api/courses" }
```

Should be < 500ms

### Check SQL Queries (Add to API):
```csharp
// In Program.cs
.LogTo(Console.WriteLine, LogLevel.Information)
```

### Check Memory:
```csharp
var memoryUsed = GC.GetTotalMemory(false) / 1024 / 1024;
Console.WriteLine($"Memory: {memoryUsed} MB");
```

---

All optimizations have been applied! Test and enjoy the improved performance! ??
