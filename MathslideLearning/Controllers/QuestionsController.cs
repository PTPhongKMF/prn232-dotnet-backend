using MathslideLearning.Business.Interfaces;
using MathslideLearning.Controllers.Base;
using MathslideLearning.Models.QuestionDtos;
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
    public class QuestionsController : ApiControllerBase
    {
        private readonly IQuestionService _questionService;

        public QuestionsController(IQuestionService questionService)
        {
            _questionService = questionService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> GetAll()
        {
            var questions = await _questionService.GetAllQuestionsAsync();
            return Api200(questions);
        }

        [HttpGet("filtered")]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> GetFilteredPaged([FromQuery] FilteredPagedQuestionRequestDto request)
        {
            try
            {
                var questions = await _questionService.GetFilteredPagedQuestionsAsync(request);
                return Api200(questions);
            }
            catch (Exception ex)
            {
                return Api400<object>(ex.Message, new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var question = await _questionService.GetQuestionByIdAsync(id);
                return Api200(question);
            }
            catch (Exception ex)
            {
                return Api404<object>(ex.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> Create([FromBody] QuestionRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return Api400<ModelStateDictionary>("Validation failed", ModelState);
            }

            try
            {
                var teacherId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var newQuestion = await _questionService.CreateQuestionAsync(request, teacherId);
                return Api201(newQuestion);
            }
            catch (Exception ex)
            {
                return Api400<object>(ex.Message, new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> Update(int id, [FromBody] QuestionRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return Api400<ModelStateDictionary>("Validation failed", ModelState);
            }

            try
            {
                var updatedQuestion = await _questionService.UpdateQuestionAsync(id, request);
                return Api200(updatedQuestion);
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
                var success = await _questionService.DeleteQuestionAsync(id);
                if (!success)
                {
                    return Api404<object>("Question not found");
                }
                return Api200<object?>("Question deleted successfully", null);
            }
            catch (Exception ex)
            {
                return Api400<object>(ex.Message, new { message = ex.Message });
            }
        }
    }
}