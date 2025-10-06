using MathslideLearning.Business.Interfaces;
using MathslideLearning.Controllers.Base;
using MathslideLearning.Models.TagDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Threading.Tasks;

namespace MathslideLearning.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TagsController : ApiControllerBase
    {
        private readonly ITagService _tagService;

        public TagsController(ITagService tagService)
        {
            _tagService = tagService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var tags = await _tagService.GetAllTagsAsync();
            return Api200(tags);
        }
        
        [HttpGet("filtered")]
        [AllowAnonymous]
        public async Task<IActionResult> GetFilteredPaged([FromQuery] FilteredPagedTagRequestDto request)
        {
            try
            {
                var tags = await _tagService.GetFilteredPagedTagsAsync(request);
                return Api200(tags);
            }
            catch (Exception ex)
            {
                return Api400<object>(ex.Message, new { message = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> Create([FromBody] TagRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return Api400<ModelStateDictionary>("Validation failed", ModelState);
            }

            try
            {
                var newTag = await _tagService.CreateTagAsync(request);
                return Api201(newTag);
            }
            catch (Exception ex)
            {
                return Api400<object>(ex.Message, new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> Update(int id, [FromBody] TagRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return Api400<ModelStateDictionary>("Validation failed", ModelState);
            }

            try
            {
                var updatedTag = await _tagService.UpdateTagAsync(id, request);
                return Api200(updatedTag);
            }
            catch (Exception ex)
            {
                return Api400<object>(ex.Message, new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var success = await _tagService.DeleteTagAsync(id);
                if (!success)
                {
                    return Api404<object>("Tag not found");
                }
                return Api200<object>("Tag deleted successfully", null);
            }
            catch (Exception ex)
            {
                return Api400<object>(ex.Message, new { message = ex.Message });
            }
        }
    }
}