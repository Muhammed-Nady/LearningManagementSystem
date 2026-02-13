using LearningManagementSystem.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace LearningManagementSystem.MVC.Controllers
{
    public class CoursesController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly ILogger<CoursesController> _logger;

        public CoursesController(IHttpClientFactory httpClientFactory, IConfiguration configuration, ILogger<CoursesController> logger)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _logger = logger;
        }

        // GET: /Courses
        [HttpGet]
        public async Task<IActionResult> Index(int? category, string? level, string? search)
        {
            var apiBase = _configuration["ApiSettings:BaseUrl"] ?? "https://localhost:7059/api/";
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(apiBase);

            var model = new CoursesIndexViewModel
            {
                SelectedCategory = category,
                SelectedLevel = level,
                SearchTerm = search
            };

            try
            {
                // Fetch courses
                var coursesResponse = await client.GetAsync("courses");
                if (coursesResponse.IsSuccessStatusCode)
                {
                    var json = await coursesResponse.Content.ReadAsStringAsync();
                    var doc = JsonDocument.Parse(json);
                    if (doc.RootElement.TryGetProperty("data", out var data))
                    {
                        foreach (var item in data.EnumerateArray())
                        {
                            model.Courses.Add(MapToCourseCard(item));
                        }
                    }
                }

                // Fetch categories
                var categoriesResponse = await client.GetAsync("categories");
                if (categoriesResponse.IsSuccessStatusCode)
                {
                    var json = await categoriesResponse.Content.ReadAsStringAsync();
                    var doc = JsonDocument.Parse(json);
                    if (doc.RootElement.TryGetProperty("data", out var data))
                    {
                        foreach (var item in data.EnumerateArray())
                        {
                            model.Categories.Add(new CategoryVm
                            {
                                CategoryId = item.GetProperty("categoryId").GetInt32(),
                                Name = item.GetProperty("name").GetString() ?? string.Empty
                            });
                        }
                    }
                }

                // Apply filters
                if (category.HasValue)
                {
                    model.Courses = model.Courses.Where(c => c.CategoryId == category.Value).ToList();
                }

                if (!string.IsNullOrEmpty(level))
                {
                    model.Courses = model.Courses.Where(c => c.Level.Equals(level, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                if (!string.IsNullOrEmpty(search))
                {
                    model.Courses = model.Courses.Where(c =>
                     c.Title.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                c.Description.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to load courses");
            }

            return View(model);
        }

        // GET: /Courses/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            // FIX: Ensure trailing slash for proper URL construction
            var apiBase = _configuration["ApiSettings:BaseUrl"] ?? "https://localhost:7059/api/";
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(apiBase);

            try
            {
                _logger.LogInformation("Fetching course details for ID: {CourseId} from {ApiBase}", id, apiBase);

                var response = await client.GetAsync($"courses/{id}");
                _logger.LogInformation("Course details API response: {StatusCode}", response.StatusCode);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    _logger.LogInformation("Course details JSON length: {Length}", json.Length);

                    var doc = JsonDocument.Parse(json);
                    if (doc.RootElement.TryGetProperty("data", out var data))
                    {
                        var model = new CourseDetailsViewModel
                        {
                            CourseId = data.GetProperty("courseId").GetInt32(),
                            Title = data.GetProperty("title").GetString() ?? string.Empty,
                            Description = data.GetProperty("description").GetString() ?? string.Empty,
                            InstructorId = data.GetProperty("instructorId").GetInt32(),
                            InstructorName = data.GetProperty("instructorName").GetString() ?? string.Empty,
                            CategoryName = data.GetProperty("categoryName").GetString() ?? string.Empty,
                            ThumbnailUrl = data.TryGetProperty("thumbnailUrl", out var t) && t.ValueKind != JsonValueKind.Null ? t.GetString() ?? string.Empty : string.Empty,
                            IsPublished = data.GetProperty("isPublished").GetBoolean(),
                            Duration = data.TryGetProperty("duration", out var d) && d.ValueKind != JsonValueKind.Null ? d.GetInt32() : null,
                            Level = data.GetProperty("level").GetString() ?? string.Empty,
                            Price = data.GetProperty("price").GetDecimal(),
                            EnrollmentCount = data.GetProperty("enrollmentCount").GetInt32(),
                            AverageRating = data.GetProperty("averageRating").GetDouble()
                        };

                        _logger.LogInformation("Successfully loaded course: {Title}", model.Title);
                        return View(model);
                    }
                    else
                    {
                        _logger.LogWarning("API response does not contain 'data' property");
                    }
                }
                else
                {
                    _logger.LogError("Course details API returned error: {StatusCode}", response.StatusCode);
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError("Error content: {Content}", errorContent);
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to load course details for ID: {CourseId}", id);
                ViewBag.Error = "Unable to load course details. Please try again later.";
                return StatusCode(500);
            }
        }

        private CourseCardVm MapToCourseCard(JsonElement item)
        {
            return new CourseCardVm
            {
                CourseId = item.GetProperty("courseId").GetInt32(),
                Title = item.GetProperty("title").GetString() ?? string.Empty,
                InstructorName = item.GetProperty("instructorName").GetString() ?? string.Empty,
                Description = item.GetProperty("description").GetString() ?? string.Empty,
                ThumbnailUrl = item.TryGetProperty("thumbnailUrl", out var t) && t.ValueKind != JsonValueKind.Null ? t.GetString() ?? string.Empty : string.Empty,
                Price = item.GetProperty("price").GetDecimal(),
                IsPublished = item.GetProperty("isPublished").GetBoolean(),
                Level = item.GetProperty("level").GetString() ?? string.Empty,
                CategoryId = item.GetProperty("categoryId").GetInt32(),
                AverageRating = item.GetProperty("averageRating").GetDouble(),
                EnrollmentCount = item.GetProperty("enrollmentCount").GetInt32()
            };
        }
    }
}
