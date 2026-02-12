using LearningManagementSystem.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace LearningManagementSystem.MVC.Controllers
{
    public class InstructorController : Controller
    {
  private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

    public InstructorController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
 {
       _httpClient = httpClientFactory.CreateClient();
  _configuration = configuration;
    _httpClient.BaseAddress = new Uri(_configuration["ApiSettings:BaseUrl"] ?? "https://localhost:7000/api");
   }

 private void SetAuthorizationHeader()
    {
 var token = Request.Cookies["AuthToken"];
         if (!string.IsNullOrEmpty(token))
     {
 _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
  }
  }

        // GET: Instructor Dashboard
        public async Task<IActionResult> Dashboard()
     {
       SetAuthorizationHeader();
  
 try
   {
      // Get instructor's courses
   var response = await _httpClient.GetAsync("/courses/instructor/me");
       if (response.IsSuccessStatusCode)
        {
  var content = await response.Content.ReadAsStringAsync();
  var result = JsonSerializer.Deserialize<ApiResponse<List<CourseResponseDto>>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
 
     var viewModel = new InstructorDashboardViewModel
     {
   Courses = result?.Data ?? new List<CourseResponseDto>()
     };
    
    return View(viewModel);
         }
   }
        catch (Exception ex)
      {
      ViewBag.Error = "Unable to load dashboard data";
        }
 
         return View(new InstructorDashboardViewModel());
        }

     // GET: My Courses
  public async Task<IActionResult> MyCourses()
        {
         SetAuthorizationHeader();
    
 try
    {
     var response = await _httpClient.GetAsync("/courses/instructor/me");
   if (response.IsSuccessStatusCode)
        {
       var content = await response.Content.ReadAsStringAsync();
 var result = JsonSerializer.Deserialize<ApiResponse<List<CourseResponseDto>>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
     
      return View(result?.Data ?? new List<CourseResponseDto>());
        }
  }
    catch (Exception ex)
            {
      ViewBag.Error = "Unable to load courses";
    }
    
          return View(new List<CourseResponseDto>());
        }

        // GET: Create Course
public async Task<IActionResult> CreateCourse()
  {
        await LoadCategories();
        return View(new CreateCourseViewModel());
 }

        // POST: Create Course
 [HttpPost]
        [ValidateAntiForgeryToken]
     public async Task<IActionResult> CreateCourse(CreateCourseViewModel model)
        {
       if (!ModelState.IsValid)
       {
     await LoadCategories();
      return View(model);
       }

          SetAuthorizationHeader();

    try
  {
   var dto = new
            {
title = model.Title,
    description = model.Description,
   categoryId = model.CategoryId,
thumbnailUrl = model.ThumbnailUrl,
     duration = model.Duration,
     level = model.Level,
price = model.Price
    };

    var json = JsonSerializer.Serialize(dto);
    var content = new StringContent(json, Encoding.UTF8, "application/json");

    var response = await _httpClient.PostAsync("/courses", content);

   if (response.IsSuccessStatusCode)
      {
      TempData["Success"] = "Course created successfully!";
       return RedirectToAction(nameof(MyCourses));
        }

     var errorContent = await response.Content.ReadAsStringAsync();
      ModelState.AddModelError("", "Failed to create course: " + errorContent);
   }
      catch (Exception ex)
 {
  ModelState.AddModelError("", "An error occurred while creating the course");
}

 await LoadCategories();
     return View(model);
  }

  // GET: Edit Course
     public async Task<IActionResult> EditCourse(int id)
   {
 SetAuthorizationHeader();

 try
  {
   var response = await _httpClient.GetAsync($"/courses/{id}");
 if (response.IsSuccessStatusCode)
           {
    var content = await response.Content.ReadAsStringAsync();
    var result = JsonSerializer.Deserialize<ApiResponse<CourseResponseDto>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

   if (result?.Data != null)
    {
   await LoadCategories();
   
       var viewModel = new EditCourseViewModel
     {
 CourseId = result.Data.CourseId,
  Title = result.Data.Title,
       Description = result.Data.Description,
      CategoryId = result.Data.CategoryId,
      ThumbnailUrl = result.Data.ThumbnailUrl,
Duration = result.Data.Duration,
    Level = result.Data.Level,
         Price = result.Data.Price,
IsPublished = result.Data.IsPublished
  };

  return View(viewModel);
   }
       }
   }
    catch (Exception ex)
        {
     TempData["Error"] = "Unable to load course details";
       }

  return RedirectToAction(nameof(MyCourses));
    }

        // POST: Edit Course
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCourse(EditCourseViewModel model)
     {
 if (!ModelState.IsValid)
       {
  await LoadCategories();
          return View(model);
  }

 SetAuthorizationHeader();

   try
  {
        var dto = new
     {
  title = model.Title,
   description = model.Description,
 categoryId = model.CategoryId,
  thumbnailUrl = model.ThumbnailUrl,
 duration = model.Duration,
   level = model.Level,
  price = model.Price
 };

    var json = JsonSerializer.Serialize(dto);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

           var response = await _httpClient.PutAsync($"/courses/{model.CourseId}", content);

     if (response.IsSuccessStatusCode)
 {
       TempData["Success"] = "Course updated successfully!";
      return RedirectToAction(nameof(MyCourses));
  }

    var errorContent = await response.Content.ReadAsStringAsync();
      ModelState.AddModelError("", "Failed to update course: " + errorContent);
}
     catch (Exception ex)
 {
      ModelState.AddModelError("", "An error occurred while updating the course");
          }

   await LoadCategories();
    return View(model);
        }

   // POST: Delete Course
        [HttpPost]
public async Task<IActionResult> DeleteCourse(int id)
 {
 SetAuthorizationHeader();

 try
         {
    var response = await _httpClient.DeleteAsync($"/courses/{id}");
     if (response.IsSuccessStatusCode)
       {
    TempData["Success"] = "Course deleted successfully!";
   }
     else
      {
    TempData["Error"] = "Failed to delete course";
  }
     }
catch (Exception ex)
        {
    TempData["Error"] = "An error occurred while deleting the course";
  }

        return RedirectToAction(nameof(MyCourses));
  }

    // POST: Publish/Unpublish Course
    [HttpPost]
      public async Task<IActionResult> TogglePublish(int id, bool publish)
        {
  SetAuthorizationHeader();

            try
            {
  var endpoint = publish ? $"/courses/{id}/publish" : $"/courses/{id}/unpublish";
     var response = await _httpClient.PostAsync(endpoint, null);

        if (response.IsSuccessStatusCode)
  {
      TempData["Success"] = publish ? "Course published successfully!" : "Course unpublished successfully!";
      }
   else
    {
    TempData["Error"] = "Failed to update course status";
 }
      }
     catch (Exception ex)
   {
       TempData["Error"] = "An error occurred";
   }

      return RedirectToAction(nameof(MyCourses));
     }

    // GET: Manage Content (Sections & Lessons)
        public async Task<IActionResult> ManageContent(int id)
        {
   SetAuthorizationHeader();

            try
    {
         var response = await _httpClient.GetAsync($"/courses/{id}");
 if (response.IsSuccessStatusCode)
 {
             var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<ApiResponse<CourseResponseDto>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

if (result?.Data != null)
 {
     return View(result.Data);
  }
           }
    }
  catch (Exception ex)
      {
 TempData["Error"] = "Unable to load course content";
   }

 return RedirectToAction(nameof(MyCourses));
        }

        private async Task LoadCategories()
  {
     try
       {
   var response = await _httpClient.GetAsync("/categories");
   if (response.IsSuccessStatusCode)
       {
 var content = await response.Content.ReadAsStringAsync();
    var result = JsonSerializer.Deserialize<ApiResponse<List<CategoryDto>>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
ViewBag.Categories = result?.Data ?? new List<CategoryDto>();
    }
    }
     catch
     {
ViewBag.Categories = new List<CategoryDto>();
  }
        }
    }
}
