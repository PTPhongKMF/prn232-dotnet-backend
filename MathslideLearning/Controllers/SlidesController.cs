using MathslideLearning.Business.Interfaces;
using MathslideLearning.Common.Models;
using MathslideLearning.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MathslideLearning.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SlidesController : ControllerBase
    {
        private readonly ISlideService _slideService;

        public SlidesController(ISlideService slideService)
        {
            _slideService = slideService;
        }
 
        [HttpGet("my-slides")]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> GetMySlides()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString)) return Unauthorized();

            var teacherId = int.Parse(userIdString);

            var slides = await _slideService.GetSlidesByTeacherIdAsync(teacherId);
            return Ok(slides);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> UpdateSlide(int id, [FromBody] SlideUpdateDto slideDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString)) return Unauthorized();

            var teacherId = int.Parse(userIdString);

            try
            {
                var updatedSlide = await _slideService.UpdateSlideAsync(id, slideDto, teacherId);
                return Ok(updatedSlide);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> DeleteSlide(int id)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString)) return Unauthorized();

            var teacherId = int.Parse(userIdString);

            try
            {
                var success = await _slideService.DeleteSlideAsync(id, teacherId);
                if (success)
                {
                    return NoContent();
                }
                return NotFound();
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
        [HttpPost]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> CreateSlide([FromBody] SlideCreateDto slideDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString))
            {
                return Unauthorized();
            }

            var teacherId = int.Parse(userIdString);

            try
            {
                var createdSlide = await _slideService.CreateSlideAsync(slideDto, teacherId);
                return CreatedAtAction(nameof(CreateSlide), new { id = createdSlide.Id }, createdSlide);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"An error occurred while creating the slide: {ex.Message}" });
            }
        }
    }
}
