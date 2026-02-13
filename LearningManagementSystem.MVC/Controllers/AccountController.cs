using LearningManagementSystem.MVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.MVC.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }


    }
}

