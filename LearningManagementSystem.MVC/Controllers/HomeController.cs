using System.Diagnostics;
using LearningManagementSystem.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace LearningManagementSystem.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new HomeViewModel();
        
            try
            {
                var apiBase = _configuration["ApiSettings:BaseUrl"] ?? "https://localhost:7059/api/";
                _logger.LogInformation("Fetching data from API: {ApiBase}", apiBase);
    
                var client = _httpClientFactory.CreateClient();
                client.BaseAddress = new Uri(apiBase);
                client.Timeout = TimeSpan.FromSeconds(30);

                var coursesResponse = await client.GetAsync("courses");
                _logger.LogInformation("Courses API Response: {StatusCode}", coursesResponse.StatusCode);

                if (coursesResponse.IsSuccessStatusCode)
                {
                    var json = await coursesResponse.Content.ReadAsStringAsync();
                    _logger.LogInformation("Courses JSON length: {Length}", json.Length);
    
                    var doc = JsonDocument.Parse(json);
                    if (doc.RootElement.TryGetProperty("data", out var coursesData))
                    {
                        var courseCount = 0;
                        foreach (var item in coursesData.EnumerateArray())
                        {
                            viewModel.FeaturedCourses.Add(new CourseCardVm
                            {
                                CourseId = item.GetProperty("courseId").GetInt32(),
                                Title = item.GetProperty("title").GetString() ?? string.Empty,
                                Description = item.GetProperty("description").GetString() ?? string.Empty,
                                InstructorName = item.GetProperty("instructorName").GetString() ?? string.Empty,
                                ThumbnailUrl = item.TryGetProperty("thumbnailUrl", out var thumb) && thumb.ValueKind != JsonValueKind.Null ? thumb.GetString() : null,
                                Price = item.GetProperty("price").GetDecimal(),
                                Level = item.GetProperty("level").GetString() ?? string.Empty,
                                CategoryId = item.GetProperty("categoryId").GetInt32(),
                                AverageRating = item.GetProperty("averageRating").GetDouble(),
                                EnrollmentCount = item.GetProperty("enrollmentCount").GetInt32(),
                                IsPublished = item.GetProperty("isPublished").GetBoolean()
                            });
                            courseCount++;
                        }
                        _logger.LogInformation("Loaded {Count} courses successfully", courseCount);
                    }
                    else
                    {
                        _logger.LogWarning("API response does not contain 'data' property");
                    }
                }
                else
                {
                    _logger.LogError("Courses API returned error: {StatusCode}", coursesResponse.StatusCode);
                    var errorContent = await coursesResponse.Content.ReadAsStringAsync();
                    _logger.LogError("Error content: {Content}", errorContent);
                }

                var categoriesResponse = await client.GetAsync("categories");
                if (categoriesResponse.IsSuccessStatusCode)
                {
                    var json = await categoriesResponse.Content.ReadAsStringAsync();
                    var doc = JsonDocument.Parse(json);
                    if (doc.RootElement.TryGetProperty("data", out var categoriesData))
                    {
                        foreach (var item in categoriesData.EnumerateArray())
                        {
                            viewModel.Categories.Add(new CategoryVm
                            {
                                CategoryId = item.GetProperty("categoryId").GetInt32(),
                                Name = item.GetProperty("name").GetString() ?? string.Empty
                            });
}
                        _logger.LogInformation("Loaded {Count} categories successfully", viewModel.Categories.Count);
                    }
}
  }
            catch (HttpRequestException hex)
            {
                _logger.LogError(hex, "HTTP error loading home page data. Is the API running?");
                ViewBag.Error = "Unable to connect to the API. Please make sure the API is running.";
            }
            catch (Exception ex)
         {
        _logger.LogError(ex, "Error loading home page data");
      ViewBag.Error = "An error occurred while loading courses. Please try again later.";
            }

        return View(viewModel);
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

