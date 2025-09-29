using MathslideLearning.Business.Interfaces;
using MathslideLearning.Controllers.Base;
using MathslideLearning.Models.ExamDtos;
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
    [Authorize]
    public class ExamsController : ApiControllerBase
    {
        private readonly IExamService _examService;

        public ExamsController(IExamService examService)
        {
            _examService = examService;
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> CreateExam([FromBody] ExamRequestDto request)
        {
            if (!ModelState.IsValid)
                return ApiBadRequest<ModelStateDictionary>(ModelState, "Validation failed");

            try
            {
                var teacherId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var newExam = await _examService.CreateExamAsync(teacherId, request);
                return ApiCreated(newExam);
            }
            catch (Exception ex)
            {
                return ApiBadRequest<object>(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> GetExamById(int id)
        {
            try
            {
                var examDto = await _examService.GetExamByIdAsync(id);
                var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                if (!User.IsInRole("Admin") && examDto.TeacherId != currentUserId)
                {
                    return ApiForbidden<object>("You don't have permission to access this exam");
                }

                return ApiOk(examDto);
            }
            catch (Exception ex)
            {
                return ApiNotFound<object>(ex.Message);
            }
        }

        [HttpGet("my-exams")]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> GetMyExams()
        {
            var teacherId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var exams = await _examService.GetExamsByTeacherAsync(teacherId);
            return ApiOk(exams);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> UpdateExam(int id, [FromBody] ExamRequestDto request)
        {
            if (!ModelState.IsValid)
                return ApiBadRequest<ModelStateDictionary>(ModelState, "Validation failed");

            try
            {
                var teacherId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var updatedExam = await _examService.UpdateExamAsync(id, teacherId, request);
                return ApiOk(updatedExam);
            }
            catch (Exception ex)
            {
                return ApiBadRequest<object>(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> DeleteExam(int id)
        {
            try
            {
                var teacherId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var success = await _examService.DeleteExamAsync(id, teacherId);
                if (!success)
                    return ApiNotFound<object>("Exam not found");
                return ApiOk<object>(null, "Exam deleted successfully");
            }
            catch (Exception ex)
            {
                return ApiBadRequest<object>(new { message = ex.Message });
            }
        }

        [HttpPost("{examId}/submit")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> SubmitExam(int examId, [FromBody] ExamSubmissionDto submission)
        {
            if (!ModelState.IsValid)
                return ApiBadRequest<ModelStateDictionary>(ModelState, "Validation failed");

            try
            {
                var studentId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var result = await _examService.SubmitExamAsync(studentId, examId, submission);
                return ApiOk(result);
            }
            catch (Exception ex)
            {
                return ApiBadRequest<object>(new { message = ex.Message });
            }
        }

        [HttpGet("history")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> GetMyExamHistory()
        {
            var studentId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var history = await _examService.GetExamHistoryForStudentAsync(studentId);
            return ApiOk(history);
        }
    }
}