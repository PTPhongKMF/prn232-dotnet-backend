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
using MathslideLearning.Models.ExamDtos;


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
                return Api400<ModelStateDictionary>("Validation failed", ModelState);

            try
            {
                var teacherId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var newExam = await _examService.CreateExamAsync(teacherId, request);
                return Api201(newExam);
            }
            catch (Exception ex)
            {
                return Api400<object>(ex.Message, new { message = ex.Message });
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
                    return Api403<object>("You don't have permission to access this exam");
                }

                return Api200(examDto);
            }
            catch (Exception ex)
            {
                return Api404<object>(ex.Message);
            }
        }

        [HttpGet("my-exams")]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> GetMyExams()
        {
            var teacherId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var exams = await _examService.GetExamsByTeacherAsync(teacherId);
            return Api200(exams);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> UpdateExam(int id, [FromBody] ExamRequestDto request)
        {
            if (!ModelState.IsValid)
                return Api400<ModelStateDictionary>("Validation failed", ModelState);

            try
            {
                var teacherId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var updatedExam = await _examService.UpdateExamAsync(id, teacherId, request);
                return Api200(updatedExam);
            }
            catch (Exception ex)
            {
                return Api400<object>(ex.Message, new { message = ex.Message });
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
                    return Api404<object>("Exam not found");
                return Api200<object>("Exam deleted successfully", null);
            }
            catch (Exception ex)
            {
                return Api400<object>(ex.Message, new { message = ex.Message });
            }
        }

        [HttpPost("{examId}/submit")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> SubmitExam(int examId, [FromBody] ExamSubmissionDto submission)
        {
            if (!ModelState.IsValid)
                return Api400<ModelStateDictionary>("Validation failed", ModelState);

            try
            {
                var studentId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var result = await _examService.SubmitExamAsync(studentId, examId, submission);
                return Api200(result);
            }
            catch (Exception ex)
            {
                return Api400<object>(ex.Message, new { message = ex.Message });
            }
        }

        [HttpGet("history")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> GetMyExamHistory()
        {
            var studentId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var history = await _examService.GetExamHistoryForStudentAsync(studentId);
            return Api200(history);
        }


        [HttpPost("{examId}/questions/add-existing")]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> AddExistingQuestionsToExam(int examId, [FromBody] AddExistingQuestionsRequestDto request)
        {
            if (request.QuestionIds == null || !request.QuestionIds.Any())
                return Api400<object>("QuestionIds cannot be empty");

            try
            {
                var result = await _examService.AddExistingQuestionsToExamAsync(examId, request.QuestionIds);
                if (!result)
                    return Api404<object>("Exam or questions not found");
                return Api200<object>("Questions added successfully", null);
            }
            catch (Exception ex)
            {
                return Api400<object>(ex.Message, new { message = ex.Message });
            }
        }

        [HttpDelete("{examId}/questions/{questionId}")]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> RemoveQuestionFromExam(int examId, int questionId)
        {
            try
            {
                var result = await _examService.RemoveQuestionFromExamAsync(examId, questionId);
                if (!result)
                    return Api404<object>("Exam or question not found");
                return Api200<object>("Question removed successfully", null);
            }
            catch (Exception ex)
            {
                return Api400<object>(ex.Message, new { message = ex.Message });
            }
        }



    }
}