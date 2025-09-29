using MathslideLearning.Business.Interfaces;
using MathslideLearning.Controllers.Base;
using MathslideLearning.Models.QuestionDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
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
            return ApiOk(questions);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var question = await _questionService.GetQuestionByIdAsync(id);
                return ApiOk(question);
            }
            catch (Exception ex)
            {
                return ApiNotFound<object>(ex.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> Create([FromBody] QuestionRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return ApiBadRequest<ModelStateDictionary>(ModelState, "Validation failed");
            }

            try
            {
                var newQuestion = await _questionService.CreateQuestionAsync(request);
                return ApiCreated(newQuestion);
            }
            catch (Exception ex)
            {
                return ApiBadRequest<object>(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> Update(int id, [FromBody] QuestionRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return ApiBadRequest<ModelStateDictionary>(ModelState, "Validation failed");
            }

            try
            {
                var updatedQuestion = await _questionService.UpdateQuestionAsync(id, request);
                return ApiOk(updatedQuestion);
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
                var success = await _questionService.DeleteQuestionAsync(id);
                if (!success)
                {
                    return ApiNotFound<object>("Question not found");
                }
                return ApiOk<object>(null, "Question deleted successfully");
            }
            catch (Exception ex)
            {
                return ApiBadRequest<object>(new { message = ex.Message });
            }
        }
    }
}