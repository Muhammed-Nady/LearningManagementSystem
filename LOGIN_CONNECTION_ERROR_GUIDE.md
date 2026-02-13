# ?? LOGIN ERROR: "Unable to connect to the server"

## Problem
When trying to login, you get: **"Unable to connect to the server. Please try again."**

---

## Root Cause
The MVC application **cannot reach the API** at `https://localhost:7059/api/auth/login`

This happens when:
1. ? API is not running
2. ? Wrong API URL in Login.cshtml
3. ? CORS blocking the request
4. ? Firewall/antivirus blocking localhost

---

## ? SOLUTION

### **Step 1: Verify API is Running**

**Check Visual Studio Output:**
```
View ? Output ? Show output from: "LearningManagementSystem.API"
```

**Should see:**
```
Now listening on: https://localhost:7059
Application started. Press Ctrl+C to shut down.
```

**If NOT running:**
```
1. In Solution Explorer
2. Right-click "LearningManagementSystem.API"
3. Select "Debug ? Start New Instance"
4. Wait for "Now listening on..." message
```

---

### **Step 2: Test API Directly**

Open browser and go to:
```
https://localhost:7059/api/courses
```

**Expected:** JSON response with courses (even if empty)
```json
{
  "success": true,
  "data": []
}
```

**If you get:**
- ? "This site can't be reached" ? API not running
- ? "ERR_CONNECTION_REFUSED" ? API not running
- ? JSON response ? API is working!

---

### **Step 3: Check Login.cshtml API URL**

Let me verify the correct API URL in Login page:

**File:** `LearningManagementSystem.MVC\Views\Account\Login.cshtml`

**Should have:**
```javascript
const API_BASE = (typeof AUTH_CONFIG !== 'undefined' && AUTH_CONFIG.API_URL) 
    ? AUTH_CONFIG.API_URL 
    : 'https://localhost:7059/api';  // ? Must be 7059!
```

**Common mistakes:**
- ? `https://localhost:7000/api` (wrong port)
- ? `https://localhost:7059` (missing /api)
- ? `http://localhost:7059/api` (missing https)

---

### **Step 4: Check Browser Console**

**While on login page:**
```
1. Press F12 (Developer Tools)
2. Go to "Console" tab
3. Try to login
4. Look for error message
```

**Common errors:**

#### **Error 1: "Failed to fetch"**
```
Failed to fetch
```
**Cause:** API not running  
**Fix:** Start API project

#### **Error 2: "net::ERR_CONNECTION_REFUSED"**
```
GET https://localhost:7059/api/auth/login net::ERR_CONNECTION_REFUSED
```
**Cause:** API not running or wrong port  
**Fix:** Start API, verify port 7059

#### **Error 3: "CORS policy"**
```
Access to fetch at 'https://localhost:7059/api/auth/login' has been blocked by CORS policy
```
**Cause:** CORS not configured  
**Fix:** Check API Program.cs (should already be configured)

#### **Error 4: "SSL certificate"**
```
NET::ERR_CERT_AUTHORITY_INVALID
```
**Cause:** Dev certificate issue  
**Fix:** Run `dotnet dev-certs https --trust`

---

## ?? DETAILED DIAGNOSTIC

### **Option 1: Use PowerShell Script**

Run the diagnostic script:
```powershell
.\DIAGNOSE_LOGIN_CONNECTION.ps1
```

### **Option 2: Manual Tests**

#### **Test API Endpoint:**
```powershell
Invoke-RestMethod -Uri "https://localhost:7059/api/auth/login" `
    -Method POST `
    -ContentType "application/json" `
    -Body '{"email":"test@test.com","password":"wrongpass"}'
```

**Expected:** 401 Unauthorized (means API is responding!)

#### **Test CORS:**
```powershell
$headers = @{
    "Origin" = "https://localhost:7012"
    "Access-Control-Request-Method" = "POST"
}
Invoke-WebRequest -Uri "https://localhost:7059/api/auth/login" -Method OPTIONS -Headers $headers
```

---

## ?? MOST COMMON FIX

### **API Not Running**

**90% of the time, this is the issue!**

**Verify both projects are running:**

```
Visual Studio ? Debug ? Windows ? Breakpoints (Ctrl+Alt+B)

You should see TWO console windows:
1. ? LearningManagementSystem.API (port 7059)
2. ? LearningManagementSystem.MVC (port 7012)
```

**If only ONE console:** Start the missing project!

---

## ? QUICK FIX STEPS

```
1. Stop both projects (Shift+F5)

2. Start API:
   - Right-click LearningManagementSystem.API
   - Debug ? Start New Instance
   - Wait for "Now listening on: https://localhost:7059"

3. Start MVC:
   - Right-click LearningManagementSystem.MVC
   - Debug ? Start New Instance
   - Browser opens to https://localhost:7012

4. Try login again

5. ? Should work!
```

---

## ?? VERIFICATION CHECKLIST

Before trying to login again:

- [ ] API running (console shows "Now listening on: https://localhost:7059")
- [ ] MVC running (browser on https://localhost:7012)
- [ ] Can access https://localhost:7059/api/courses in browser
- [ ] Browser console (F12) shows no errors
- [ ] Login.cshtml has correct API URL (7059, not 7000)

---

## ?? IF STILL NOT WORKING

### **Check Network Tab:**

```
1. F12 ? Network tab
2. Clear (trash icon)
3. Try to login
4. Look for /api/auth/login request
```

**If NO request appears:**
- JavaScript error preventing fetch
- Check Console tab for errors

**If request appears but fails:**
- Click on the request
- Check "Response" tab
- Share the error message

---

## ?? STATUS CHECK

Run this in PowerShell to check status:

```powershell
Write-Host "Checking API..." -ForegroundColor Yellow
try {
    $api = Invoke-WebRequest "https://localhost:7059/api/courses" -UseBasicParsing
    Write-Host "? API Running" -ForegroundColor Green
} catch {
    Write-Host "? API NOT Running" -ForegroundColor Red
}

Write-Host "Checking MVC..." -ForegroundColor Yellow
try {
    $mvc = Invoke-WebRequest "https://localhost:7012" -UseBasicParsing
    Write-Host "? MVC Running" -ForegroundColor Green
} catch {
    Write-Host "? MVC NOT Running" -ForegroundColor Red
}
```

---

## ? EXPECTED BEHAVIOR

### **When Login Works:**

```
1. Enter email & password
2. Click "Sign In"
3. Button shows spinner: "Signing in..."
4. (Behind scenes: POST to https://localhost:7059/api/auth/login)
5. API returns JWT token
6. Token saved to localStorage & cookies
7. Redirect to homepage
8. Navbar shows user menu
```

### **Network Request:**

```
POST https://localhost:7059/api/auth/login
Status: 200 OK
Response:
{
  "success": true,
  "data": {
    "token": "eyJhbGc...",
    "userId": 1,
    "email": "student@example.com",
    "firstName": "John",
    "role": "Student"
  }
}
```

---

## ?? MOST LIKELY CAUSE

Based on "Unable to connect to the server" error:

**99% chance:** API is not running!

**Quick test:**
```
Open: https://localhost:7059/api/courses

If you see: "This site can't be reached"
? API is NOT running
? Start it!
```

---

## ?? TIP: Configure Multiple Startup Projects

To start both at once:

```
1. Right-click Solution
2. Properties
3. Startup Project ? Multiple startup projects
4. Set both API and MVC to "Start"
5. OK
6. Press F5 ? Both start together!
```

---

## ?? SUMMARY

**Error:** "Unable to connect to the server"  
**Cause:** API not running or not reachable  
**Fix:** Start API project  
**Verify:** Can access https://localhost:7059/api/courses  

**Run diagnostic script:** `.\DIAGNOSE_LOGIN_CONNECTION.ps1`

---

**Tell me what you see when you:**
1. Visit `https://localhost:7059/api/courses` in browser
2. Check Visual Studio Output for API
3. Press F12 on login page and check Console

Then I can provide the exact fix! ??
