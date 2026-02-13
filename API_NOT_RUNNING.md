# ?? CRITICAL: API IS NOT RUNNING!

## The Problem:
Your HomeController is trying to connect to `https://localhost:7059/api/courses`, but **the API is not running!**

This is why `coursesResponse.IsSuccessStatusCode` is always **false** - it can't even connect to the API.

---

## ? Solution: Start the API Project

### Option 1: Configure Multiple Startup Projects (Best)

1. **Right-click on the Solution** (top item in Solution Explorer)
2. Click **Properties**
3. Select **Multiple startup projects**
4. Set both projects to **Start**:
   - ? `LearningManagementSystem.API` ? **Start**
   - ? `LearningManagementSystem.MVC` ? **Start**
5. Click **OK**
6. Press **F5** to start both projects

### Option 2: Start API Manually

1. **Right-click** `LearningManagementSystem.API` project
2. Select **Debug** ? **Start New Instance**
3. Wait for API to start (Swagger page should open)
4. **Then** start the MVC project

### Option 3: Use Command Line

```powershell
# Terminal 1: Start API
cd LearningManagementSystem.API
dotnet run

# Terminal 2: Start MVC (in new terminal)
cd LearningManagementSystem.MVC
dotnet run
```

---

## How to Verify API is Running

### Check 1: Look for Swagger
After starting API, browser should open: **https://localhost:7059/swagger**

### Check 2: Check Process
```powershell
Get-Process | Where-Object {$_.ProcessName -like "*LearningManagement*"}
```
Should show 2 processes (API and MVC)

### Check 3: Test Endpoint
Open in browser: **https://localhost:7059/api/courses**

Should see JSON with courses like:
```json
{
  "success": true,
  "data": [
    {
      "courseId": 1,
      "title": "Complete React Developer Course",
 ...
    }
  ]
}
```

---

## Why This Happens

### Common Reasons API Doesn't Start:

1. **Only MVC is set as startup project**
   - Solution: Set Multiple Startup Projects (see above)

2. **API has compilation errors**
   - Check Output window for errors
   - Run: `dotnet build LearningManagementSystem.API`

3. **Port 7059 is already in use**
   - Check: `netstat -ano | findstr 7059`
   - Kill process if needed

4. **Database connection fails**
   - API won't start if it can't connect to database
   - Check connection string in API appsettings.json

---

## What Happens When API Runs

When you start the API successfully, you'll see:

```
Now listening on: https://localhost:7059
Now listening on: http://localhost:5015
Application started. Press Ctrl+C to shut down.
```

And Swagger UI will open in browser at: https://localhost:7059/swagger

---

## After Starting API

Once both projects are running:

1. **Refresh homepage**: https://localhost:7012
2. **Check Output window** (Ctrl+Alt+O):
   - Select `LearningManagementSystem.MVC`
   - Should now see: `"Courses API Response: OK"`
   - Should see: `"Loaded 14 courses successfully"`
3. **Homepage should show courses!**

---

## Quick Checklist

? API project is set to start (Multiple Startup Projects)  
? API builds without errors  
? Database is accessible (connection string correct)  
? Port 7059 is not blocked by firewall  
? Both projects start when you press F5  
? Swagger opens at https://localhost:7059/swagger  
? MVC opens at https://localhost:7012  

---

## Still Not Working?

### Check API Output Window:
1. **View** ? **Output** (Ctrl+Alt+O)
2. Select **LearningManagementSystem.API** from dropdown
3. Look for error messages

### Common Errors:

**"Unable to bind to https://localhost:7059"**
? Port is already in use. Kill the process or change port.

**"Unable to connect to database"**
? Check SQL Server is running and connection string is correct.

**"The type initializer for 'Microsoft.EntityFrameworkCore.Query.QueryableMethods' threw an exception"**
? Database migration issue. Run: `dotnet ef database update`

---

## Summary

**The reason `coursesResponse.IsSuccessStatusCode` is false:**
? **API is not running!** ?

**Solution:**
? **Start the API project** ?

**After starting API:**
? HomeController will successfully connect ?
? `IsSuccessStatusCode` will be `true` ?
? Courses will load on homepage ?

---

**Start the API now, then refresh your homepage!** ??
