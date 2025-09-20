using MathslideLearning.Business.Interfaces;
using MathslideLearning.Models.ExamDtos;
using MathslideLearning.Models.QuestionDtos;
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
    public class ExamsController : ControllerBase
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
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var teacherId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var newExam = await _examService.CreateExamAsync(teacherId, request);
                return CreatedAtAction(nameof(GetExamById), new { id = newExam.Id }, newExam);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
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
                    return Forbid();
                }

                return Ok(examDto);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("my-exams")]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> GetMyExams()
        {
            var teacherId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var exams = await _examService.GetExamsByTeacherAsync(teacherId);
            return Ok(exams);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> UpdateExam(int id, [FromBody] ExamRequestDto request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var teacherId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var updatedExam = await _examService.UpdateExamAsync(id, teacherId, request);
                return Ok(updatedExam);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
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
                if (!success) return NotFound();
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("{examId}/submit")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> SubmitExam(int examId, [FromBody] ExamSubmissionDto submission)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var studentId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var result = await _examService.SubmitExamAsync(studentId, examId, submission);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("history")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> GetMyExamHistory()
        {
            var studentId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var history = await _examService.GetExamHistoryForStudentAsync(studentId);
            return Ok(history);
        }
    }
}

