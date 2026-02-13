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

        /// <summary>
        /// Get all published courses (Public)
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        // TODO: Re-enable caching in production with cache invalidation strategy
        // [ResponseCache(Duration = 300, Location = ResponseCacheLocation.Any, VaryByQueryKeys = new[] { "*" })]
        public async Task<IActionResult> GetAllPublishedCourses()
        {
            var result = await _courseService.GetAllPublishedCoursesAsync();
            return Ok(result);
        }

        /// <summary>
        /// Get course by ID
        /// </summary>
        [HttpGet("{id}")]
        [AllowAnonymous]
        // TODO: Re-enable caching in production
        // [ResponseCache(Duration = 600, Location = ResponseCacheLocation.Any, VaryByQueryKeys = new[] { "id" })]
        public async Task<IActionResult> GetCourseById(int id)
        {
            var result = await _courseService.GetCourseByIdAsync(id);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        /// <summary>
        /// Get current instructor's courses
        /// </summary>
        [HttpGet("instructor/me")]
        [Authorize(Roles = "Instructor,Admin")]
        public async Task<IActionResult> GetMyInstructorCourses()
        {
            var instructorId = GetCurrentUserId();
            var result = await _courseService.GetCoursesByInstructorAsync(instructorId);
            return Ok(result);
        }

        /// <summary>
        /// Get courses by instructor ID
        /// </summary>
        [HttpGet("instructor/{instructorId}")]
        // TODO: Re-enable caching in production
        // [ResponseCache(Duration = 300, Location = ResponseCacheLocation.Any, VaryByQueryKeys = new[] { "instructorId" })]
        public async Task<IActionResult> GetCoursesByInstructor(int instructorId)
        {
            var result = await _courseService.GetCoursesByInstructorAsync(instructorId);
            return Ok(result);
        }

        /// <summary>
        /// Get courses by category ID
        /// </summary>
        [HttpGet("category/{categoryId}")]
        [AllowAnonymous]
        // TODO: Re-enable caching in production
        // [ResponseCache(Duration = 300, Location = ResponseCacheLocation.Any, VaryByQueryKeys = new[] { "categoryId" })]
        public async Task<IActionResult> GetCoursesByCategory(int categoryId)
        {
            var result = await _courseService.GetCoursesByCategoryAsync(categoryId);
            return Ok(result);
        }

        /// <summary>
        /// Create a new course (Instructor or Admin only)
        /// </summary>
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

        /// <summary>
        /// Update course (Instructor or Admin only)
        /// </summary>
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

        /// <summary>
        /// Delete course (Instructor or Admin only)
        /// </summary>
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

        /// <summary>
        /// Publish course (Instructor or Admin only)
        /// </summary>
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

        /// <summary>
        /// Unpublish course (Instructor or Admin only)
        /// </summary>
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
