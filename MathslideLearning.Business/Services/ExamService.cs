using MathslideLearning.Business.Interfaces;
using MathslideLearning.Data.Entities;
using MathslideLearning.Data.Interfaces;
using MathslideLearning.Models.ExamDtos;
using MathslideLearning.Models.QuestionDtos;
using MathslideLearning.Models.TagDtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace MathslideLearning.Business.Services
{
    public class ExamService : IExamService
    {
        private readonly IExamRepository _examRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IUserExamHistoryRepository _historyRepository;

        public ExamService(IExamRepository examRepository, IQuestionRepository questionRepository, IUserExamHistoryRepository historyRepository)
        {
            _examRepository = examRepository;
            _questionRepository = questionRepository;
            _historyRepository = historyRepository;
        }

        public async Task<ExamResponseDto> CreateExamAsync(int teacherId, ExamRequestDto request)
        {
            foreach (var questionId in request.QuestionIds)
            {
                if (await _questionRepository.GetByIdAsync(questionId) == null)
                    throw new Exception($"Question with ID {questionId} not found.");
            }

            var newExam = new Exam
            {
                Name = request.Name,
                Content = request.Content,
                TeacherId = teacherId,
                ExamQuestions = request.QuestionIds.Select(qId => new ExamQuestion { QuestionId = qId }).ToList()
            };

            var createdExam = await _examRepository.CreateAsync(newExam);
            return await GetExamByIdAsync(createdExam.Id);
        }

        public async Task<ExamResponseDto> GetExamByIdAsync(int examId)
        {
            var exam = await _examRepository.GetByIdAsync(examId);
            if (exam == null) throw new Exception("Exam not found.");
            return MapToExamDetailDto(exam); 
        }

        public async Task<IEnumerable<ExamResponseDto>> GetExamsByTeacherAsync(int teacherId)
        {
   
            var allExams = await _examRepository.GetAllAsync();

            // 🔹 Lọc theo TeacherId
            var exams = allExams
                .Where(e => e.TeacherId == teacherId)
                .ToList();

            // 🔹 Ánh xạ sang DTO
            var result = exams.Select(e => new ExamResponseDto
            {
                Id = e.Id,
                Name = e.Name,
                Content = e.Content,
                TeacherId = e.TeacherId,
                TeacherName = e.Teacher?.Name ?? "Unknown",
                QuestionsCount = e.ExamQuestions?.Count ?? 0,
                Questions = e.ExamQuestions?.Select(eq => new QuestionResponseDto
                {
                    Id = eq.QuestionId,
                    Content = eq.Question?.Content,
                    Type = eq.Question?.Type
                }).ToList() ?? new List<QuestionResponseDto>()
            });

            return result;
        }

        public async Task<ExamResponseDto> UpdateExamAsync(int examId, int teacherId, ExamRequestDto request)
        {
            var examToUpdate = await _examRepository.GetByIdAsync(examId);
            if (examToUpdate == null) throw new Exception("Exam not found.");
            if (examToUpdate.TeacherId != teacherId) throw new Exception("You are not authorized to edit this exam.");

            foreach (var questionId in request.QuestionIds)
            {
                if (await _questionRepository.GetByIdAsync(questionId) == null)
                    throw new Exception($"Question with ID {questionId} not found.");
            }

            examToUpdate.Name = request.Name;
            examToUpdate.Content = request.Content;

            examToUpdate.ExamQuestions.Clear();
            foreach (var qId in request.QuestionIds)
            {
                examToUpdate.ExamQuestions.Add(new ExamQuestion { QuestionId = qId });
            }

            await _examRepository.UpdateAsync(examToUpdate);
            return await GetExamByIdAsync(examId);
        }

        public async Task<bool> DeleteExamAsync(int examId, int teacherId)
        {
            var examToDelete = await _examRepository.GetByIdAsync(examId);
            if (examToDelete == null) return false;
            if (examToDelete.TeacherId != teacherId) throw new Exception("You are not authorized to delete this exam.");

            return await _examRepository.DeleteAsync(examId);
        }

        // --- Student Methods ---
        public async Task<ExamResultDto> SubmitExamAsync(int studentId, int examId, ExamSubmissionDto submission)
        {
            var exam = await _examRepository.GetByIdAsync(examId);
            if (exam == null) throw new Exception("Exam not found.");

            int score = 0;
            var correctAnswers = exam.ExamQuestions
                .SelectMany(eq => eq.Question.Answers.Where(a => a.IsCorrect))
                .ToDictionary(a => a.QuestionId, a => a.Id);

            foreach (var submittedAnswer in submission.Answers)
            {
                if (correctAnswers.TryGetValue(submittedAnswer.QuestionId, out int correctAnswerId) && submittedAnswer.AnswerId == correctAnswerId)
                {
                    score++;
                }
            }

            var history = new UserExamHistory
            {
                UserId = studentId,
                ExamId = examId,
                Score = score,
                SubmittedAt = DateTime.UtcNow,
                Content = JsonSerializer.Serialize(submission.Answers)
            };

            await _historyRepository.CreateAsync(history);

            return new ExamResultDto
            {
                ExamId = examId,
                ExamName = exam.Name,
                Score = score,
                TotalQuestions = exam.ExamQuestions.Count,
                SubmittedAt = history.SubmittedAt
            };
        }

        public async Task<IEnumerable<UserExamHistoryDto>> GetExamHistoryForStudentAsync(int studentId)
        {
            var history = await _historyRepository.GetByUserIdAsync(studentId);
            return history.Select(h => new UserExamHistoryDto
            {
                Id = h.Id,
                ExamId = h.ExamId,
                ExamName = h.Exam.Name,
                Score = h.Score,
                SubmittedAt = h.SubmittedAt,
                Content = h.Content
            });
        }

        private ExamResponseDto MapToExamDetailDto(Exam exam)
        {
            return new ExamResponseDto
            {
                Id = exam.Id,
                Name = exam.Name,
                Content = exam.Content,
                TeacherName = exam.Teacher.Name,
                Questions = exam.ExamQuestions.Select(eq => new QuestionResponseDto
                {
                    Id = eq.Question.Id,
                    Content = eq.Question.Content,
                    Type = eq.Question.Type,
                    Answers = eq.Question.Answers.Select(a => new AnswerDto { Content = a.Content, IsCorrect = a.IsCorrect }).ToList(),
                    Tags = eq.Question.QuestionTags.Select(qt => new TagDto { Id = qt.Tag.Id, Name = qt.Tag.Name }).ToList()
                }).ToList()
            };
        }

        public async Task<bool> AddExistingQuestionsToExamAsync(int examId, List<int> questionIds)
        {
         
            var exam = await _examRepository.GetByIdAsync(examId);
            if (exam == null) return false;

           
            foreach (var qId in questionIds)
            {
                if (await _questionRepository.GetByIdAsync(qId) == null)
                    throw new Exception($"Question with ID {qId} not found.");
            }

            return await _examRepository.AddQuestionsToExamAsync(examId, questionIds);
        }

        public async Task<bool> RemoveQuestionFromExamAsync(int examId, int questionId)
        {
            return await _examRepository.RemoveQuestionFromExamAsync(examId, questionId);
        }
    }
}

