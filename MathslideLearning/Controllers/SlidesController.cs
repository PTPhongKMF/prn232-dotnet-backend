using MathslideLearning.Business.Interfaces;
using MathslideLearning.Controllers.Base;
using MathslideLearning.Models.SlideDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MathslideLearning.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SlidesController : ApiControllerBase
    {
        private readonly ISlideService _slideService;

        public SlidesController(ISlideService slideService)
        {
            _slideService = slideService;
        }

        private int GetTeacherId()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString))
            {
                throw new UnauthorizedAccessException("User ID not found in token");
            }
            return int.Parse(userIdString);
        }

        [HttpGet("my-slides")]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> GetMySlides()
        {
            try
            {
                var teacherId = GetTeacherId();
                var slides = await _slideService.GetSlidesByTeacherIdAsync(teacherId);
                return ApiOk(slides);
            }
            catch (UnauthorizedAccessException ex)
            {
                return ApiUnauthorized<object>(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> UpdateSlide(int id, [FromBody] SlideUpdateDto slideDto)
        {
            if (!ModelState.IsValid)
            {
                return ApiBadRequest<ModelStateDictionary>(ModelState, "Validation failed");
            }

            try
            {
                var teacherId = GetTeacherId();
                var updatedSlide = await _slideService.UpdateSlideAsync(id, slideDto, teacherId);
                return ApiOk(updatedSlide);
            }
            catch (UnauthorizedAccessException ex)
            {
                return ApiForbidden<object>(ex.Message);
            }
            catch (Exception ex)
            {
                return ApiNotFound<object>(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> DeleteSlide(int id)
        {
            try
            {
                var teacherId = GetTeacherId();
                var success = await _slideService.DeleteSlideAsync(id, teacherId);
                if (success)
                {
                    return ApiOk<object>(null, "Slide deleted successfully");
                }
                return ApiNotFound<object>("Slide not found");
            }
            catch (UnauthorizedAccessException ex)
            {
                return ApiForbidden<object>(ex.Message);
            }
            catch (Exception ex)
            {
                return ApiNotFound<object>(ex.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> CreateSlide([FromBody] SlideCreateDto slideDto)
        {
            if (!ModelState.IsValid)
            {
                return ApiBadRequest<ModelStateDictionary>(ModelState, "Validation failed");
            }

            try
            {
                var teacherId = GetTeacherId();
                var createdSlide = await _slideService.CreateSlideAsync(slideDto, teacherId);
                return ApiCreated(createdSlide);
            }
            catch (UnauthorizedAccessException ex)
            {
                return ApiUnauthorized<object>(ex.Message);
            }
            catch (Exception ex)
            {
                return ApiBadRequest<object>(new { message = $"An error occurred while creating the slide: {ex.Message}" });
            }
        }
    }
}