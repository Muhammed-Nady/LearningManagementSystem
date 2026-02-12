using LearningManagementSystem.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace LearningManagementSystem.MVC.Controllers
{
    public class StudentController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly ILogger<StudentController> _logger;

        public StudentController(IHttpClientFactory httpClientFactory, IConfiguration configuration, ILogger<StudentController> logger)
 {
            _httpClientFactory = httpClientFactory;
          _configuration = configuration;
            _logger = logger;
        }

        // GET: /Student/Dashboard
        public async Task<IActionResult> Dashboard()
{
     var apiBase = _configuration["ApiSettings:BaseUrl"] ?? "https://localhost:7000/api";
        var client = _httpClientFactory.CreateClient("ApiClient");
        client.BaseAddress = new Uri(apiBase);

       var model = new StudentDashboardViewModel();

            // Note: In production, get userId from authenticated user
            // For now, using placeholder - will be replaced with auth token parsing
            
return View(model);
        }

        // GET: /Student/MyCourses
        public async Task<IActionResult> MyCourses()
        {
            var model = new MyCoursesViewModel();
       return View(model);
     }

  // GET: /Student/Learn/5
        public async Task<IActionResult> Learn(int id)
        {
     var apiBase = _configuration["ApiSettings:BaseUrl"] ?? "https://localhost:7000/api";
   var client = _httpClientFactory.CreateClient("ApiClient");
  client.BaseAddress = new Uri(apiBase);

      try
     {
    var response = await client.GetAsync($"/courses/{id}");
      if (response.IsSuccessStatusCode)
        {
     var json = await response.Content.ReadAsStringAsync();
          var doc = JsonDocument.Parse(json);
           if (doc.RootElement.TryGetProperty("data", out var data))
     {
       var model = new LearnViewModel
  {
   CourseId = data.GetProperty("courseId").GetInt32(),
         CourseTitle = data.GetProperty("title").GetString() ?? string.Empty,
            InstructorName = data.GetProperty("instructorName").GetString() ?? string.Empty
   };

             return View(model);
   }
    }

                return NotFound();
            }
  catch (Exception ex)
            {
    _logger.LogError(ex, "Failed to load learning page");
          return StatusCode(500);
     }
        }
    }
}
