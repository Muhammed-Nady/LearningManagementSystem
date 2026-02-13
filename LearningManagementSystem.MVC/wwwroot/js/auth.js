// Authentication Helper
// Manages JWT tokens and user authentication state

const AUTH_CONFIG = {
    API_URL: 'https://localhost:7059/api',
    TOKEN_KEY: 'lms_auth_token',
    USER_KEY: 'lms_user_data'
};

// Cookie helper functions
function setCookie(name, value, days = 7) {
    const expires = new Date();
    expires.setTime(expires.getTime() + days * 24 * 60 * 60 * 1000);
    document.cookie = `${name}=${value};expires=${expires.toUTCString()};path=/;SameSite=Lax`;
}

function getCookie(name) {
    const nameEQ = name + "=";
    const ca = document.cookie.split(';');
for (let i = 0; i < ca.length; i++) {
        let c = ca[i];
        while (c.charAt(0) === ' ') c = c.substring(1, c.length);
        if (c.indexOf(nameEQ) === 0) return c.substring(nameEQ.length, c.length);
    }
  return null;
}

function deleteCookie(name) {
  document.cookie = `${name}=;expires=Thu, 01 Jan 1970 00:00:00 UTC;path=/;`;
}

// Get stored token (check both localStorage and cookie)
function getToken() {
    return localStorage.getItem(AUTH_CONFIG.TOKEN_KEY) || getCookie('AuthToken');
}

// Get stored user data
function getUser() {
    const userData = localStorage.getItem(AUTH_CONFIG.USER_KEY);
    return userData ? JSON.parse(userData) : null;
}

// Save authentication data (to both localStorage AND cookies)
function saveAuth(token, userData) {
    // Save to localStorage (for JavaScript access)
    localStorage.setItem(AUTH_CONFIG.TOKEN_KEY, token);
    localStorage.setItem(AUTH_CONFIG.USER_KEY, JSON.stringify(userData));
    
  // Save to cookies (for server-side access)
    setCookie('AuthToken', token, 7); // 7 days expiry
    setCookie('UserRole', userData.role, 7);
    setCookie('UserEmail', userData.email, 7);
    setCookie('UserName', userData.firstName, 7);
    
    updateNavigation();
}

// Clear authentication data (from both localStorage AND cookies)
function clearAuth() {
    // Clear localStorage
  localStorage.removeItem(AUTH_CONFIG.TOKEN_KEY);
    localStorage.removeItem(AUTH_CONFIG.USER_KEY);
    
    // Clear cookies
    deleteCookie('AuthToken');
    deleteCookie('UserRole');
    deleteCookie('UserEmail');
    deleteCookie('UserName');
    
    updateNavigation();
}

// Check if user is authenticated
function isAuthenticated() {
    return !!getToken();
}

// Get current user role
function getUserRole() {
    const user = getUser();
    return user ? user.role : (getCookie('UserRole') || null);
}

// Logout function
function logout() {
    if (confirm('Are you sure you want to logout?')) {
        clearAuth();
        window.location.href = '/';
    }
}

// Update navigation based on auth state
function updateNavigation() {
    const isAuth = isAuthenticated();
    const user = getUser();
    const userRole = getUserRole();

    // Elements
    const navLogin = document.getElementById('navLogin');
    const navRegister = document.getElementById('navRegister');
    const navUserMenu = document.getElementById('navUserMenu');
    const userName = document.getElementById('userName');
    
    // Role-specific dashboards
    const navStudentDashboard = document.getElementById('navStudentDashboard');
    const navInstructorDashboard = document.getElementById('navInstructorDashboard');
    const navAdminDashboard = document.getElementById('navAdminDashboard');

    if (isAuth) {
        // Show authenticated elements
        if (navLogin) navLogin.classList.add('d-none');
        if (navRegister) navRegister.classList.add('d-none');
        if (navUserMenu) navUserMenu.classList.remove('d-none');
      if (userName) userName.textContent = (user?.firstName || getCookie('UserName') || 'User');

        // Show/hide role-specific dashboard links
 if (navStudentDashboard) {
    userRole === 'Student' ? navStudentDashboard.classList.remove('d-none') : navStudentDashboard.classList.add('d-none');
        }
if (navInstructorDashboard) {
   userRole === 'Instructor' ? navInstructorDashboard.classList.remove('d-none') : navInstructorDashboard.classList.add('d-none');
      }
        if (navAdminDashboard) {
            userRole === 'Admin' ? navAdminDashboard.classList.remove('d-none') : navAdminDashboard.classList.add('d-none');
   }

     // Show/hide dropdown menu items based on role
    document.querySelectorAll('.role-student').forEach(el => {
      userRole === 'Student' ? el.classList.remove('d-none') : el.classList.add('d-none');
  });
      document.querySelectorAll('.role-instructor').forEach(el => {
            userRole === 'Instructor' ? el.classList.remove('d-none') : el.classList.add('d-none');
        });
document.querySelectorAll('.role-admin').forEach(el => {
      userRole === 'Admin' ? el.classList.remove('d-none') : el.classList.add('d-none');
        });
    } else {
  // Show non-authenticated elements
        if (navLogin) navLogin.classList.remove('d-none');
    if (navRegister) navRegister.classList.remove('d-none');
        if (navUserMenu) navUserMenu.classList.add('d-none');
        if (navStudentDashboard) navStudentDashboard.classList.add('d-none');
        if (navInstructorDashboard) navInstructorDashboard.classList.add('d-none');
        if (navAdminDashboard) navAdminDashboard.classList.add('d-none');
    }
}

// Initialize on page load
document.addEventListener('DOMContentLoaded', function () {
    updateNavigation();
});

// Redirect if not authenticated
function requireAuth() {
    if (!isAuthenticated()) {
        window.location.href = '/account/login?returnUrl=' + encodeURIComponent(window.location.pathname);
     return false;
    }
    return true;
}

// Redirect if not specific role
function requireRole(requiredRole) {
    if (!requireAuth()) return false;

    const userRole = getUserRole();
    if (userRole !== requiredRole) {
        alert('Access denied. You do not have permission to access this page.');
        window.location.href = '/';
     return false;
    }
    return true;
}

// Check if token is expired
function isTokenExpired() {
    const user = getUser();
    if (!user || !user.expiresAt) return true;

    const expiryDate = new Date(user.expiresAt);
    return expiryDate < new Date();
}

// Auto-refresh token if needed (optional enhancement)
async function refreshTokenIfNeeded() {
    if (isAuthenticated() && isTokenExpired()) {
        console.log('Token expired, please login again');
    clearAuth();
        window.location.href = '/account/login';
    }
}

// Check token validity periodically
setInterval(refreshTokenIfNeeded, 60000); // Check every minute
