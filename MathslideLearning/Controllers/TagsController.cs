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
            return ApiOk(tags);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> Create([FromBody] TagRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return ApiBadRequest<ModelStateDictionary>(ModelState, "Validation failed");
            }

            try
            {
                var newTag = await _tagService.CreateTagAsync(request);
                return ApiCreated(newTag);
            }
            catch (Exception ex)
            {
                return ApiBadRequest<object>(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> Update(int id, [FromBody] TagRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return ApiBadRequest<ModelStateDictionary>(ModelState, "Validation failed");
            }

            try
            {
                var updatedTag = await _tagService.UpdateTagAsync(id, request);
                return ApiOk(updatedTag);
            }
            catch (Exception ex)
            {
                return ApiBadRequest<object>(new { message = ex.Message });
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
                    return ApiNotFound<object>("Tag not found");
                }
                return ApiOk<object>(null, "Tag deleted successfully");
            }
            catch (Exception ex)
            {
                return ApiBadRequest<object>(new { message = ex.Message });
            }
        }
    }
}