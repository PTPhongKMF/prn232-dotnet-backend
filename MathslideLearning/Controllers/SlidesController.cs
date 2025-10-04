using MathslideLearning.Business.Interfaces;
using MathslideLearning.Controllers.Base;
using MathslideLearning.Models.SlideDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Security.Claims;
using System.Text.Json;
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
        [HttpGet("public")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllPublicSlides()
        {
            var slides = await _slideService.GetAllPublicSlidesAsync();
            return Api200(slides);
        }

        [HttpPatch("{id}/status")]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> UpdateSlideStatus(int id, [FromBody] SlideStatusUpdateDto request)
        {
            if (!ModelState.IsValid)
            {
                return Api400<ModelStateDictionary>("Validation failed", ModelState);
            }

            try
            {
                var teacherId = GetTeacherId();
                var updatedSlide = await _slideService.UpdateSlideStatusAsync(id, teacherId, request.IsPublished);
                return Api200(updatedSlide);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Api403<object>(ex.Message);
            }
            catch (Exception ex)
            {
                return Api404<object>(ex.Message);
            }
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

        [HttpPost]
        [Authorize(Roles = "Teacher")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreateSlide([FromForm] string slideDtoStr, [FromForm] IFormFile file)
        {
            if (string.IsNullOrEmpty(slideDtoStr))
            {
                return Api400<object>("Slide data is required.");
            }

            var slideDto = JsonSerializer.Deserialize<SlideCreateDto>(slideDtoStr, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (!ModelState.IsValid)
            {
                return Api400<ModelStateDictionary>("Validation failed", ModelState);
            }

            try
            {
                var teacherId = GetTeacherId();
                var createdSlide = await _slideService.CreateSlideAsync(slideDto, teacherId, file);
                return Api201(createdSlide);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Api401<object>(ex.Message);
            }
            catch (Exception ex)
            {
                return Api400<object>($"An error occurred while creating the slide: {ex.Message}", new { message = $"An error occurred while creating the slide: {ex.Message}" });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Teacher")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateSlide(int id, [FromForm] string slideDtoStr, [FromForm] IFormFile? file)
        {
            if (string.IsNullOrEmpty(slideDtoStr))
            {
                return Api400<object>("Slide data is required.");
            }

            var slideDto = JsonSerializer.Deserialize<SlideUpdateDto>(slideDtoStr, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (!ModelState.IsValid)
            {
                return Api400<ModelStateDictionary>("Validation failed", ModelState);
            }

            try
            {
                var teacherId = GetTeacherId();
                var updatedSlide = await _slideService.UpdateSlideAsync(id, slideDto, teacherId, file);
                return Api200(updatedSlide);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Api403<object>(ex.Message);
            }
            catch (Exception ex)
            {
                return Api404<object>(ex.Message);
            }
        }

        [HttpGet("my-slides")]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> GetMySlides()
        {
            try
            {
                var teacherId = GetTeacherId();
                var slides = await _slideService.GetSlidesByTeacherIdAsync(teacherId);
                return Api200(slides);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Api401<object>(ex.Message);
            }
        }

        [HttpGet("user/{userId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetSlidesByUserId(int userId)
        {
            var slides = await _slideService.GetSlidesByTeacherIdAsync(userId);
            return Api200(slides);
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
                    return Api200<object>("Slide deleted successfully", null);
                }
                return Api404<object>("Slide not found");
            }
            catch (UnauthorizedAccessException ex)
            {
                return Api403<object>(ex.Message);
            }
            catch (Exception ex)
            {
                return Api404<object>(ex.Message);
            }
        }
    }
}