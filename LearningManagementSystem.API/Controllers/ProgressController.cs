using LearningManagementSystem.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LearningManagementSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Student")]
    public class ProgressController : ControllerBase
    {
        private readonly IProgressService _progressService;

        public ProgressController(IProgressService progressService)
      {
      _progressService = progressService;
        }



     [HttpPost("lessons/{lessonId}/complete")]
        public async Task<IActionResult> MarkLessonComplete(int lessonId)
{
            var studentId = GetCurrentUserId();
   var result = await _progressService.MarkLessonCompleteAsync(studentId, lessonId);

     if (!result.Success)
  return BadRequest(result);

   return Ok(result);
     }



        [HttpGet("courses/{courseId}")]
        public async Task<IActionResult> GetCourseProgress(int courseId)
   {
   var studentId = GetCurrentUserId();
 var result = await _progressService.CalculateCourseProgressAsync(studentId, courseId);

        return Ok(result);
        }



  [HttpGet("courses/{courseId}/last-lesson")]
public async Task<IActionResult> GetLastAccessedLesson(int courseId)
        {
   var studentId = GetCurrentUserId();
      var result = await _progressService.GetLastAccessedLessonAsync(studentId, courseId);

  return Ok(result);
 }

        private int GetCurrentUserId()
  {
       var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
       return int.Parse(userIdClaim ?? "0");
    }
    }
}

