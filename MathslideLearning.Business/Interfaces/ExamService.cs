using MathslideLearning.Business.Interfaces;
using MathslideLearning.Data.Entities;
using MathslideLearning.Data.Interfaces;
using MathslideLearning.Models.ExamDtos;
using MathslideLearning.Models.QuestionDtos;
using MathslideLearning.Models.TagDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MathslideLearning.Business.Services
{
    public class ExamService : IExamService
    {
        private readonly IExamRepository _examRepository;
        private readonly IQuestionRepository _questionRepository;

        public ExamService(IExamRepository examRepository, IQuestionRepository questionRepository)
        {
            _examRepository = examRepository;
            _questionRepository = questionRepository;
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
            return MapToResponseDto(exam);
        }

        public async Task<IEnumerable<ExamResponseDto>> GetExamsByTeacherAsync(int teacherId)
        {
            var exams = await _examRepository.GetByTeacherIdAsync(teacherId);
            return exams.Select(e => new ExamResponseDto { Id = e.Id, Name = e.Name, Content = e.Content });
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

        private ExamResponseDto MapToResponseDto(Exam exam)
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
    }
}
