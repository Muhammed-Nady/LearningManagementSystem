using LearningManagementSystem.Core.DTOs.Course;
using LearningManagementSystem.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LearningManagementSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseService _courseService;

        public CoursesController(ICourseService courseService)
        {
            _courseService = courseService;
        }



        [HttpGet]
        [AllowAnonymous]


        public async Task<IActionResult> GetAllPublishedCourses()
        {
            var result = await _courseService.GetAllPublishedCoursesAsync();
            return Ok(result);
        }



        [HttpGet("{id}")]
        [AllowAnonymous]


        public async Task<IActionResult> GetCourseById(int id)
        {
            var result = await _courseService.GetCourseByIdAsync(id);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }



        [HttpGet("instructor/me")]
        [Authorize(Roles = "Instructor,Admin")]
        public async Task<IActionResult> GetMyInstructorCourses()
        {
            var instructorId = GetCurrentUserId();
            var result = await _courseService.GetCoursesByInstructorAsync(instructorId);
            return Ok(result);
        }



        [HttpGet("instructor/{instructorId}")]


        public async Task<IActionResult> GetCoursesByInstructor(int instructorId)
        {
            var result = await _courseService.GetCoursesByInstructorAsync(instructorId);
            return Ok(result);
        }



        [HttpGet("category/{categoryId}")]
        [AllowAnonymous]


        public async Task<IActionResult> GetCoursesByCategory(int categoryId)
        {
            var result = await _courseService.GetCoursesByCategoryAsync(categoryId);
            return Ok(result);
        }



        [HttpPost]
        [Authorize(Roles = "Instructor,Admin")]
        public async Task<IActionResult> CreateCourse([FromBody] CreateCourseDto dto)
        {
            var instructorId = GetCurrentUserId();
            var result = await _courseService.CreateCourseAsync(dto, instructorId);

            if (!result.Success)
                return BadRequest(result);

            return CreatedAtAction(nameof(GetCourseById), new { id = result.Data!.CourseId }, result);
        }



        [HttpPut("{id}")]
        [Authorize(Roles = "Instructor,Admin")]
        public async Task<IActionResult> UpdateCourse(int id, [FromBody] UpdateCourseDto dto)
        {
            var instructorId = GetCurrentUserId();
            var result = await _courseService.UpdateCourseAsync(id, dto, instructorId);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }



        [HttpDelete("{id}")]
        [Authorize(Roles = "Instructor,Admin")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var instructorId = GetCurrentUserId();
            var result = await _courseService.DeleteCourseAsync(id, instructorId);

            if (!result.Success)
                return BadRequest(result);

            return NoContent();
        }



        [HttpPost("{id}/publish")]
        [Authorize(Roles = "Instructor,Admin")]
        public async Task<IActionResult> PublishCourse(int id)
        {
            var instructorId = GetCurrentUserId();
            var result = await _courseService.PublishCourseAsync(id, instructorId);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }



        [HttpPost("{id}/unpublish")]
        [Authorize(Roles = "Instructor,Admin")]
        public async Task<IActionResult> UnpublishCourse(int id)
        {
            var instructorId = GetCurrentUserId();
            var result = await _courseService.UnpublishCourseAsync(id, instructorId);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.Parse(userIdClaim ?? "0");
        }
    }
}

