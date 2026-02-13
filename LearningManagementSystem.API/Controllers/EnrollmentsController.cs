using LearningManagementSystem.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LearningManagementSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EnrollmentsController : ControllerBase
    {
     private readonly IEnrollmentService _enrollmentService;

        public EnrollmentsController(IEnrollmentService enrollmentService)
        {
 _enrollmentService = enrollmentService;
  }



[HttpPost("{courseId}")]
  [Authorize(Roles = "Student")]
        public async Task<IActionResult> EnrollInCourse(int courseId)
        {
   var studentId = GetCurrentUserId();
            var result = await _enrollmentService.EnrollStudentAsync(studentId, courseId);

  if (!result.Success)
return BadRequest(result);

  return Ok(result);
     }



   [HttpDelete("{courseId}")]
        [Authorize(Roles = "Student")]
    public async Task<IActionResult> UnenrollFromCourse(int courseId)
        {
var studentId = GetCurrentUserId();
    var result = await _enrollmentService.UnenrollStudentAsync(studentId, courseId);

   if (!result.Success)
    return BadRequest(result);

     return NoContent();
   }



     [HttpGet("{courseId}/check")]
   [Authorize(Roles = "Student")]
        public async Task<IActionResult> CheckEnrollment(int courseId)
        {
            var studentId = GetCurrentUserId();
  var result = await _enrollmentService.IsStudentEnrolledAsync(studentId, courseId);

 return Ok(result);
  }



        [HttpGet("my-courses")]
   [Authorize(Roles = "Student")]
        public async Task<IActionResult> GetMyEnrolledCourses()
     {
       var studentId = GetCurrentUserId();
     var result = await _enrollmentService.GetStudentCourseIdsAsync(studentId);

            return Ok(result);
        }

      private int GetCurrentUserId()
  {
    var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
   return int.Parse(userIdClaim ?? "0");
        }
  }
}

