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
     // FIX: Update port to 7059 and ensure trailing slash
     var apiBase = _configuration["ApiSettings:BaseUrl"] ?? "https://localhost:7059/api/";
      var client = _httpClientFactory.CreateClient();
 client.BaseAddress = new Uri(apiBase);

    var model = new StudentDashboardViewModel();

    try
    {
   // Get auth token from cookie
      var token = Request.Cookies["AuthToken"];
          if (!string.IsNullOrEmpty(token))
          {
     client.DefaultRequestHeaders.Authorization = 
  new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
    }
      else
          {
     // If not authenticated, redirect to login
     return RedirectToAction("Login", "Account", new { returnUrl = "/Student/Dashboard" });
      }

      _logger.LogInformation("Fetching student dashboard data");

      // Get enrolled course IDs
       var enrollmentsResponse = await client.GetAsync("enrollments/my-courses");
            
   if (enrollmentsResponse.IsSuccessStatusCode)
      {
     var enrollmentsJson = await enrollmentsResponse.Content.ReadAsStringAsync();
       var enrollmentsDoc = JsonDocument.Parse(enrollmentsJson);
       
   if (enrollmentsDoc.RootElement.TryGetProperty("data", out var courseIds))
         {
   var courseIdsList = courseIds.EnumerateArray().Select(c => c.GetInt32()).ToList();
           model.TotalEnrolledCourses = courseIdsList.Count;

     // Fetch recent courses (limit to 3)
     foreach (var id in courseIdsList.Take(3))
 {
     var courseResponse = await client.GetAsync($"courses/{id}");
    if (courseResponse.IsSuccessStatusCode)
      {
        var courseJson = await courseResponse.Content.ReadAsStringAsync();
    var courseDoc = JsonDocument.Parse(courseJson);
         
       if (courseDoc.RootElement.TryGetProperty("data", out var courseData))
     {
 model.RecentCourses.Add(new EnrolledCourseVm
 {
    CourseId = courseData.GetProperty("courseId").GetInt32(),
       Title = courseData.GetProperty("title").GetString() ?? string.Empty,
 InstructorName = courseData.GetProperty("instructorName").GetString() ?? string.Empty,
       ThumbnailUrl = courseData.TryGetProperty("thumbnailUrl", out var thumb) 
     && thumb.ValueKind != JsonValueKind.Null 
           ? thumb.GetString() ?? string.Empty 
           : string.Empty,
          ProgressPercentage = 0, // TODO: Get from progress API
     Status = "Active",
      EnrolledAt = DateTime.UtcNow
      });
      }
          }
    }

  model.ContinueLearning = new List<EnrolledCourseVm>(model.RecentCourses);
  _logger.LogInformation("Loaded student dashboard with {Count} courses", model.TotalEnrolledCourses);
    }
    }
   else if (enrollmentsResponse.StatusCode == System.Net.HttpStatusCode.Unauthorized)
       {
   _logger.LogWarning("Unauthorized access to dashboard");
    return RedirectToAction("Login", "Account", new { returnUrl = "/Student/Dashboard" });
  }
      }
    catch (HttpRequestException hex)
      {
   _logger.LogError(hex, "HTTP error loading student dashboard");
     ViewBag.Error = "Unable to connect to the server.";
        }
 catch (Exception ex)
   {
    _logger.LogError(ex, "Failed to load student dashboard");
   ViewBag.Error = "An error occurred while loading your dashboard.";
        }

return View(model);
        }

        // GET: /Student/MyCourses
        public async Task<IActionResult> MyCourses()
        {
var apiBase = _configuration["ApiSettings:BaseUrl"] ?? "https://localhost:7059/api/";
       var client = _httpClientFactory.CreateClient();
         client.BaseAddress = new Uri(apiBase);

            var model = new MyCoursesViewModel();

         try
         {
 // Get auth token from cookie
   var token = Request.Cookies["AuthToken"];
    if (!string.IsNullOrEmpty(token))
                {
   client.DefaultRequestHeaders.Authorization = 
     new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
           }
                else
     {
           // If not authenticated, redirect to login
       return RedirectToAction("Login", "Account", new { returnUrl = "/Student/MyCourses" });
    }

 _logger.LogInformation("Fetching enrolled courses for student");

         // Get enrolled course IDs
            var enrollmentsResponse = await client.GetAsync("enrollments/my-courses");
             _logger.LogInformation("Enrollments API response: {StatusCode}", enrollmentsResponse.StatusCode);

            if (enrollmentsResponse.IsSuccessStatusCode)
     {
      var enrollmentsJson = await enrollmentsResponse.Content.ReadAsStringAsync();
  _logger.LogInformation("Enrollments JSON: {Json}", enrollmentsJson);

      var enrollmentsDoc = JsonDocument.Parse(enrollmentsJson);
            if (enrollmentsDoc.RootElement.TryGetProperty("data", out var courseIds))
     {
   // Fetch details for each enrolled course
         foreach (var courseId in courseIds.EnumerateArray())
        {
         var id = courseId.GetInt32();
        var courseResponse = await client.GetAsync($"courses/{id}");
        
      if (courseResponse.IsSuccessStatusCode)
    {
  var courseJson = await courseResponse.Content.ReadAsStringAsync();
       var courseDoc = JsonDocument.Parse(courseJson);
        
          if (courseDoc.RootElement.TryGetProperty("data", out var courseData))
           {
        model.EnrolledCourses.Add(new EnrolledCourseVm
      {
          CourseId = courseData.GetProperty("courseId").GetInt32(),
    Title = courseData.GetProperty("title").GetString() ?? string.Empty,
      InstructorName = courseData.GetProperty("instructorName").GetString() ?? string.Empty,
     ThumbnailUrl = courseData.TryGetProperty("thumbnailUrl", out var thumb) 
       && thumb.ValueKind != JsonValueKind.Null 
  ? thumb.GetString() ?? string.Empty 
        : string.Empty,
             ProgressPercentage = 0, // TODO: Get from progress API
      Status = "Active",
     EnrolledAt = DateTime.UtcNow // TODO: Get from enrollment data
      });
            }
     }
         }

  _logger.LogInformation("Loaded {Count} enrolled courses", model.EnrolledCourses.Count);
      }
 else
        {
   _logger.LogWarning("Enrollments response does not contain 'data' property");
  }
                }
         else if (enrollmentsResponse.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
      _logger.LogWarning("Unauthorized access to enrollments");
    return RedirectToAction("Login", "Account", new { returnUrl = "/Student/MyCourses" });
    }
     else
         {
    _logger.LogError("Enrollments API returned error: {StatusCode}", enrollmentsResponse.StatusCode);
  var errorContent = await enrollmentsResponse.Content.ReadAsStringAsync();
     _logger.LogError("Error content: {Content}", errorContent);
    ViewBag.Error = "Unable to load your courses. Please try again later.";
  }
            }
        catch (HttpRequestException hex)
        {
       _logger.LogError(hex, "HTTP error loading enrolled courses");
           ViewBag.Error = "Unable to connect to the server. Please check your connection.";
     }
     catch (Exception ex)
         {
     _logger.LogError(ex, "Failed to load enrolled courses");
    ViewBag.Error = "An error occurred while loading your courses. Please try again later.";
            }

     return View(model);
        }

  // GET: /Student/Learn/5
        public async Task<IActionResult> Learn(int id)
    {
  // FIX: Update port to 7059 and ensure trailing slash
   var apiBase = _configuration["ApiSettings:BaseUrl"] ?? "https://localhost:7059/api/";
   var client = _httpClientFactory.CreateClient();
  client.BaseAddress = new Uri(apiBase);

      try
    {
    // REMOVED leading slash
    var response = await client.GetAsync($"courses/{id}");
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
