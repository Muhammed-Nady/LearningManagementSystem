using LearningManagementSystem.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace LearningManagementSystem.MVC.Controllers
{
    public class AdminController : Controller
    {
  private readonly HttpClient _httpClient;
private readonly IConfiguration _configuration;

        public AdminController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
  {
    _httpClient = httpClientFactory.CreateClient();
 _configuration = configuration;

   _httpClient.BaseAddress = new Uri(_configuration["ApiSettings:BaseUrl"] ?? "https://localhost:7059/api/");
     }

        private void SetAuthorizationHeader()
      {
        var token = Request.Cookies["AuthToken"];
   if (!string.IsNullOrEmpty(token))
     {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
}
        }

        public async Task<IActionResult> Dashboard()
  {
       SetAuthorizationHeader();

try
     {

  var usersResponse = await _httpClient.GetAsync("users");
 var coursesResponse = await _httpClient.GetAsync("courses");
   var categoriesResponse = await _httpClient.GetAsync("categories");

       var viewModel = new AdminDashboardViewModel();

      if (usersResponse.IsSuccessStatusCode)
   {
  var content = await usersResponse.Content.ReadAsStringAsync();
  var result = JsonSerializer.Deserialize<ApiResponse<List<UserDto>>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
     viewModel.TotalUsers = result?.Data?.Count ?? 0;
     viewModel.ActiveUsers = result?.Data?.Count(u => u.IsActive) ?? 0;
         }

if (coursesResponse.IsSuccessStatusCode)
      {
     var content = await coursesResponse.Content.ReadAsStringAsync();
      var result = JsonSerializer.Deserialize<ApiResponse<List<CourseResponseDto>>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
viewModel.TotalCourses = result?.Data?.Count ?? 0;
        viewModel.PublishedCourses = result?.Data?.Count(c => c.IsPublished) ?? 0;
 }

      if (categoriesResponse.IsSuccessStatusCode)
   {
     var content = await categoriesResponse.Content.ReadAsStringAsync();
     var result = JsonSerializer.Deserialize<ApiResponse<List<CategoryDto>>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
   viewModel.TotalCategories = result?.Data?.Count ?? 0;
     }

   return View(viewModel);
      }
  catch (Exception ex)
   {
     ViewBag.Error = "Unable to load dashboard data";
              return View(new AdminDashboardViewModel());
   }
}

   public async Task<IActionResult> ManageUsers()
        {
   SetAuthorizationHeader();

       try
  {

      var response = await _httpClient.GetAsync("users");
    if (response.IsSuccessStatusCode)
      {
var content = await response.Content.ReadAsStringAsync();
    var result = JsonSerializer.Deserialize<ApiResponse<List<UserDto>>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
  return View(result?.Data ?? new List<UserDto>());
         }
 }
  catch (Exception ex)
      {
          ViewBag.Error = "Unable to load users";
 }

  return View(new List<UserDto>());
     }

      [HttpPost]
    public async Task<IActionResult> ActivateUser(int id)
 {
  SetAuthorizationHeader();

      try
    {

   var response = await _httpClient.PostAsync($"users/{id}/activate", null);
  if (response.IsSuccessStatusCode)
       {
    TempData["Success"] = "User activated successfully!";
      }
     else
    {
   TempData["Error"] = "Failed to activate user";
    }
}
    catch (Exception ex)
        {
     TempData["Error"] = "An error occurred";
  }

   return RedirectToAction(nameof(ManageUsers));
 }

    [HttpPost]
public async Task<IActionResult> DeactivateUser(int id)
      {
SetAuthorizationHeader();

  try
    {

   var response = await _httpClient.PostAsync($"users/{id}/deactivate", null);
     if (response.IsSuccessStatusCode)
{
     TempData["Success"] = "User deactivated successfully!";
}
        else
  {
    TempData["Error"] = "Failed to deactivate user";
       }
   }
       catch (Exception ex)
        {
     TempData["Error"] = "An error occurred";
  }

     return RedirectToAction(nameof(ManageUsers));
   }

     public async Task<IActionResult> ManageCategories()
        {
   SetAuthorizationHeader();

    try
   {

  var response = await _httpClient.GetAsync("categories");
   if (response.IsSuccessStatusCode)
  {
     var content = await response.Content.ReadAsStringAsync();
     var result = JsonSerializer.Deserialize<ApiResponse<List<CategoryDto>>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
return View(result?.Data ?? new List<CategoryDto>());
       }
   }
    catch (Exception ex)
  {
     ViewBag.Error = "Unable to load categories";
    }

return View(new List<CategoryDto>());
   }

     [HttpPost]
   [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCategory(string name, string? description)
        {
 SetAuthorizationHeader();

     try
      {
   var dto = new { name, description };
     var json = JsonSerializer.Serialize(dto);
 var content = new StringContent(json, Encoding.UTF8, "application/json");

   var response = await _httpClient.PostAsync("categories", content);
    if (response.IsSuccessStatusCode)
      {
        TempData["Success"] = "Category created successfully!";
        }
else
       {
  TempData["Error"] = "Failed to create category";
   }
    }
         catch (Exception ex)
   {
       TempData["Error"] = "An error occurred";
     }

     return RedirectToAction(nameof(ManageCategories));
    }

  [HttpPost]
  public async Task<IActionResult> DeleteCategory(int id)
      {
     SetAuthorizationHeader();

     try
   {

var response = await _httpClient.DeleteAsync($"categories/{id}");
    if (response.IsSuccessStatusCode)
      {
   TempData["Success"] = "Category deleted successfully!";
    }
   else
 {
  TempData["Error"] = "Failed to delete category";
    }
  }
 catch (Exception ex)
      {
    TempData["Error"] = "An error occurred";
 }

        return RedirectToAction(nameof(ManageCategories));
     }
    }

public class UserDto
    {
  public int UserId { get; set; }
     public string Email { get; set; } = string.Empty;
      public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
      public string Role { get; set; } = string.Empty;
    public bool IsActive { get; set; }
 public DateTime CreatedAt { get; set; }
    }
}

