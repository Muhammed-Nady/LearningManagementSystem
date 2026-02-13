using LearningManagementSystem.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LearningManagementSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ReviewsController : ControllerBase
    {
    private readonly IReviewService _reviewService;

        public ReviewsController(IReviewService reviewService)
   {
    _reviewService = reviewService;
        }



        [HttpGet("course/{courseId}")]
        [AllowAnonymous]
      public async Task<IActionResult> GetCourseReviews(int courseId)
  {
    var result = await _reviewService.GetCourseReviewsAsync(courseId);
     return Ok(result);
        }



        [HttpGet("course/{courseId}/rating")]
        [AllowAnonymous]
     public async Task<IActionResult> GetCourseAverageRating(int courseId)
  {
      var result = await _reviewService.GetCourseAverageRatingAsync(courseId);
   return Ok(result);
   }



     [HttpPost]
        [Authorize(Roles = "Student")]
  public async Task<IActionResult> SubmitReview([FromBody] SubmitReviewRequest request)
        {
            var studentId = GetCurrentUserId();
  var result = await _reviewService.SubmitReviewAsync(
    studentId, 
   request.CourseId, 
  request.Rating, 
      request.Comment);

            if (!result.Success)
       return BadRequest(result);

     return Ok(result);
    }



    [HttpDelete("{reviewId}")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> DeleteReview(int reviewId)
 {
         var studentId = GetCurrentUserId();
   var result = await _reviewService.DeleteReviewAsync(reviewId, studentId);

        if (!result.Success)
       return BadRequest(result);

      return NoContent();
        }

   private int GetCurrentUserId()
  {
       var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
          return int.Parse(userIdClaim ?? "0");
        }
    }

    public class SubmitReviewRequest
    {
        public int CourseId { get; set; }
     public int Rating { get; set; }
        public string? Comment { get; set; }
    }
}

