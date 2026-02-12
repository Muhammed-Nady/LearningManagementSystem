// Authentication Helper
// Manages JWT tokens and user authentication state

const AUTH_CONFIG = {
    API_URL: 'https://localhost:7059/api',
    TOKEN_KEY: 'lms_auth_token',
    USER_KEY: 'lms_user_data'
};

// Get stored token
function getToken() {
    return localStorage.getItem(AUTH_CONFIG.TOKEN_KEY);
}

// Get stored user data
function getUser() {
    const userData = localStorage.getItem(AUTH_CONFIG.USER_KEY);
    return userData ? JSON.parse(userData) : null;
}

// Save authentication data
function saveAuth(token, userData) {
    localStorage.setItem(AUTH_CONFIG.TOKEN_KEY, token);
    localStorage.setItem(AUTH_CONFIG.USER_KEY, JSON.stringify(userData));
    updateNavigation();
}

// Clear authentication data
function clearAuth() {
    localStorage.removeItem(AUTH_CONFIG.TOKEN_KEY);
    localStorage.removeItem(AUTH_CONFIG.USER_KEY);
    updateNavigation();
}

// Check if user is authenticated
function isAuthenticated() {
    return !!getToken();
}

// Get current user role
function getUserRole() {
    const user = getUser();
    return user ? user.role : null;
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

  if (isAuth && user) {
        // Show authenticated elements
        if (navLogin) navLogin.classList.add('d-none');
        if (navRegister) navRegister.classList.add('d-none');
        if (navUserMenu) navUserMenu.classList.remove('d-none');
        if (userName) userName.textContent = user.firstName || 'User';

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
