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
    [Route("api/slides/{slideId}/pages")]
    [Authorize(Roles = "Teacher")]
    public class SlidePagesController : ApiControllerBase
    {
        private readonly ISlidePageService _slidePageService;

        public SlidePagesController(ISlidePageService slidePageService)
        {
            _slidePageService = slidePageService;
        }

        private int GetCurrentUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                throw new InvalidOperationException("User ID not found in token.");
            }
            return int.Parse(userId);
        }

        [HttpPost]
        public async Task<IActionResult> AddPage(int slideId, [FromBody] SlidePageRequestDto pageDto)
        {
            if (!ModelState.IsValid)
            {
                return Api400<ModelStateDictionary>("Validation failed", ModelState);
            }

            try
            {
                var teacherId = GetCurrentUserId();
                var newPage = await _slidePageService.AddPageToSlideAsync(slideId, teacherId, pageDto);
                return Api201(newPage);
            }
            catch (Exception ex)
            {
                return Api400<object>(ex.Message, new { message = ex.Message });
            }
        }

        [HttpPut("{pageId}")]
        public async Task<IActionResult> UpdatePage(int slideId, int pageId, [FromBody] SlidePageRequestDto pageDto)
        {
            if (!ModelState.IsValid)
            {
                return Api400<ModelStateDictionary>("Validation failed", ModelState);
            }

            try
            {
                var teacherId = GetCurrentUserId();
                var updatedPage = await _slidePageService.UpdateSlidePageAsync(slideId, pageId, teacherId, pageDto);
                return Api200(updatedPage);
            }
            catch (Exception ex)
            {
                return Api400<object>(ex.Message, new { message = ex.Message });
            }
        }

        [HttpDelete("{pageId}")]
        public async Task<IActionResult> DeletePage(int slideId, int pageId)
        {
            try
            {
                var teacherId = GetCurrentUserId();
                await _slidePageService.DeleteSlidePageAsync(slideId, pageId, teacherId);
                return Api200<object>("Page deleted successfully", null);
            }
            catch (Exception ex)
            {
                return Api400<object>(ex.Message, new { message = ex.Message });
            }
        }

        //[HttpGet("{pageId}")]
        //public IActionResult GetPage(int slideId, int pageId)
        //{
        //    return Api200(new { slideId, pageId }, $"Details for page {pageId} in slide {slideId}");
        //}
    }
}