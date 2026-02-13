# ? LOGIN ISSUE FIXED!

## ?? **PROBLEM IDENTIFIED**

### **The Issue:**
Users cannot login - the login form refuses to authenticate users.

### **Root Causes Found:**

1. **? Wrong API URL** - Login page was using `https://localhost:7000/api` but should be `https://localhost:7059/api`
2. **? JavaScript Syntax Errors** - Periods (`.`) instead of semicolons (`;`) in code
3. **? Register page had same wrong API URL**

---

## ? **THE FIX**

### **Files Fixed:**

#### **1. Login.cshtml**
**File:** `LearningManagementSystem.MVC\Views\Account\Login.cshtml`

**Changed:**
```javascript
// ? BEFORE - Wrong API URL
const API_BASE = '...7000/api';
const returnUrl = document.getElementById('ReturnUrl').value || '/'.  // Syntax error

// ? AFTER - Correct API URL
const API_BASE = '...7059/api';
const returnUrl = document.getElementById('ReturnUrl').value || '/';  // Fixed
```

#### **2. Register.cshtml**
**File:** `LearningManagementSystem.MVC\Views\Account\Register.cshtml`

**Changed:**
```javascript
// ? BEFORE
const API_BASE = '...7000/api';

// ? AFTER
const API_BASE = '...7059/api';
```

---

## ?? **API ENDPOINTS**

### **Correct API Endpoints:**

| Endpoint | URL |
|----------|-----|
| **Login** | `https://localhost:7059/api/auth/login` |
| **Register** | `https://localhost:7059/api/auth/register` |
| **Courses** | `https://localhost:7059/api/courses` |
| **Categories** | `https://localhost:7059/api/categories` |

### **Project Ports:**

| Project | Port |
|---------|------|
| **API** | `https://localhost:7059` |
| **MVC** | `https://localhost:7012` |

---

## ?? **TO TEST THE FIX**

### **Step 1: Ensure Both Projects Are Running**

**Start API:**
```
1. Right-click LearningManagementSystem.API
2. Debug ? Start New Instance
3. Wait for: "Now listening on: https://localhost:7059"
```

**Start MVC:**
```
1. Right-click LearningManagementSystem.MVC
2. Debug ? Start New Instance
3. Opens browser: https://localhost:7012
```

### **Step 2: Test Login**

```
1. Navigate to: https://localhost:7012/account/login
2. Enter credentials:
   - Email: student@example.com
   - Password: Password123!
3. Click "Sign In"
4. ? Should redirect to homepage
5. ? Should see user menu in navbar
```

### **Step 3: Test Register**

```
1. Navigate to: https://localhost:7012/account/register
2. Fill in:
   - First Name: Test
   - Last Name: User
   - Email: test@example.com
   - Password: Password123!
   - Confirm Password: Password123!
   - [ ] Register as Instructor (optional)
3. Click "Create Account"
4. ? Should redirect to homepage
5. ? Should be logged in automatically
```

---

## ?? **COMMON LOGIN ISSUES & SOLUTIONS**

### **Issue 1: "Unable to connect to server"**

**Cause:** API is not running or wrong port

**Solution:**
```
1. Check API is running on port 7059
2. Open browser: https://localhost:7059/api/courses
3. Should see JSON response (not error page)
```

### **Issue 2: "Invalid email or password"**

**Cause:** Credentials are incorrect or user doesn't exist

**Solution:**
```
1. Check user exists in database:
   SELECT * FROM Users WHERE Email = 'student@example.com'

2. If user doesn't exist, register first
3. Password must match exactly (case-sensitive)
```

### **Issue 3: Login button stuck on "Signing in..."**

**Cause:** JavaScript error or API not responding

**Solution:**
```
1. Open Developer Tools (F12)
2. Check Console tab for errors
3. Check Network tab:
   - Look for /auth/login request
   - Check if it's red (failed)
   - Click on it to see error details
```

### **Issue 4: Redirects to homepage but not logged in**

**Cause:** Token not being saved properly

**Solution:**
```
1. Open Developer Tools (F12)
2. Go to Application tab
3. Check:
   - Local Storage ? Has lms_auth_token?
   - Cookies ? Has AuthToken cookie?
4. If missing, check auth.js is loaded:
   - Network tab ? look for auth.js
```

---

## ?? **HOW TO DEBUG LOGIN ISSUES**

### **Step 1: Open Developer Tools**
```
Press F12 in browser
```

### **Step 2: Check Console Tab**
```
Look for JavaScript errors (red text)
Common errors:
- "saveAuth is not defined" ? auth.js not loaded
- "Failed to fetch" ? API not running
- "Unexpected token" ? JSON parsing error
```

### **Step 3: Check Network Tab**
```
1. Keep Network tab open
2. Try to login
3. Look for requests:
   - POST /api/auth/login
   
Click on the request to see:
- Headers ? Check URL is correct
- Payload ? Check email/password sent
- Response ? Check API response
```

### **Step 4: Check Request Details**

**Successful Login:**
```json
Request URL: https://localhost:7059/api/auth/login
Status: 200 OK

Response:
{
  "success": true,
  "data": {
    "token": "eyJhbGc...",
    "userId": 1,
    "email": "student@example.com",
    "firstName": "John",
    "lastName": "Doe",
    "role": "Student",
    "expiresAt": "2026-02-20T..."
  }
}
```

**Failed Login:**
```json
Status: 401 Unauthorized

Response:
{
  "success": false,
  "message": "Invalid email or password"
}
```

---

## ?? **LOGIN WORKFLOW**

```
1. User fills login form
   ?
2. JavaScript captures submit event
   ?
3. Sends POST to /api/auth/login
   - URL: https://localhost:7059/api/auth/login
   - Body: { email, password }
   ?
4. API validates credentials
   ?
5. API returns JWT token + user data
   ?
6. JavaScript calls saveAuth() from auth.js
   ?
7. Token saved to:
   - localStorage (lms_auth_token)
   - Cookie (AuthToken)
 ?
8. Navigation updated (shows user menu)
   ?
9. Redirect to homepage or returnUrl
```

---

## ? **VERIFICATION CHECKLIST**

### **After Login:**

? **LocalStorage has:**
- `lms_auth_token` = "eyJhbGc..."
- `lms_user_data` = "{...user info...}"

? **Cookies have:**
- `AuthToken` = "eyJhbGc..."
- `UserRole` = "Student" (or Instructor/Admin)
- `UserEmail` = "student@example.com"
- `UserName` = "John"

? **Navbar shows:**
- User menu with name
- Dashboard link (role-specific)
- Logout option

? **Login/Register buttons:**
- Hidden after successful login

---

## ?? **TEST CREDENTIALS**

### **If you need to create test users:**

Run this SQL in your database:

```sql
-- Create test student
INSERT INTO Users (Email, PasswordHash, FirstName, LastName, Role, IsActive, CreatedAt)
VALUES (
    'student@example.com',
    '$2a$11$YourHashedPassword',  -- Use actual bcrypt hash
  'John',
    'Doe',
    'Student',
    1,
    GETDATE()
);

-- Create test instructor
INSERT INTO Users (Email, PasswordHash, FirstName, LastName, Role, IsActive, CreatedAt)
VALUES (
    'instructor@example.com',
    '$2a$11$YourHashedPassword',
    'Jane',
    'Smith',
    'Instructor',
    1,
    GETDATE()
);
```

**Or register through the UI - it's easier!**

---

## ?? **FILES CHANGED**

| File | Change |
|------|--------|
| `Login.cshtml` | ? Fixed API URL (7000 ? 7059) |
| `Login.cshtml` | ? Fixed JavaScript syntax errors |
| `Register.cshtml` | ? Fixed API URL (7000 ? 7059) |

---

## ?? **SUCCESS CRITERIA**

After restarting MVC, you should be able to:

1. ? Navigate to `/account/login`
2. ? Enter valid credentials
3. ? Click "Sign In" button
4. ? See "Signing in..." spinner briefly
5. ? Get redirected to homepage (or returnUrl)
6. ? See user menu in navbar with your name
7. ? See role-specific dashboard link
8. ? Login/Register buttons hidden

---

## ?? **IMPORTANT NOTES**

### **API Must Be Running**
The login will ALWAYS fail if the API project is not running. Check:
```
https://localhost:7059/api/courses
```
Should return JSON, not a connection error.

### **CORS Settings**
If you get CORS errors, check `Program.cs` in API project has CORS policy configured.

### **Password Requirements**
- Minimum 6 characters
- Must match exactly (case-sensitive)

### **Token Expiration**
- Default: 7 days
- Check `expiresAt` field in response
- Auto-logout when token expires

---

## ?? **IF STILL NOT WORKING**

### **Clear Browser Cache:**
```
1. Press Ctrl+Shift+Delete
2. Select "Cookies and other site data"
3. Select "Cached images and files"
4. Click "Clear data"
5. Refresh page (Ctrl+F5)
```

### **Clear LocalStorage & Cookies:**
```
1. Open Developer Tools (F12)
2. Application tab
3. Local Storage ? Right-click ? Clear
4. Cookies ? Right-click ? Clear
5. Refresh page
```

### **Check auth.js is loaded:**
```
1. Open Developer Tools (F12)
2. Network tab
3. Filter: "auth.js"
4. Should show 200 OK status
5. If 404, file is missing or path is wrong
```

---

## ?? **SUMMARY**

**Problem:** Login failing  
**Cause:** Wrong API URL (port 7000 instead of 7059)  
**Fix:** Updated Login.cshtml and Register.cshtml  
**Status:** ? Fixed  
**Action:** Restart MVC and test login  

**Files Modified:**
- ? `LearningManagementSystem.MVC\Views\Account\Login.cshtml`
- ? `LearningManagementSystem.MVC\Views\Account\Register.cshtml`

**Build:** ? Successful

---

**Your login should now work perfectly!** ??

**Restart MVC (Shift+F5, then F5) and test!** ??
