using MathslideLearning.Business.Interfaces;
using MathslideLearning.Data.Entities;
using MathslideLearning.Data.Interfaces;
using MathslideLearning.Models.QuestionDtos;
using MathslideLearning.Models.TagDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MathslideLearning.Business.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly IQuestionRepository _questionRepository;

        public QuestionService(IQuestionRepository questionRepository)
        {
            _questionRepository = questionRepository;
        }

        public async Task<QuestionResponseDto> CreateQuestionAsync(QuestionRequestDto request)
        {
            if (!request.Answers.Any(a => a.IsCorrect))
            {
                throw new Exception("A question must have at least one correct answer.");
            }

            var newQuestion = new Question
            {
                Content = request.Content,
                Type = request.Type,
                Answers = request.Answers.Select(a => new Answer { Content = a.Content, IsCorrect = a.IsCorrect }).ToList(),
                QuestionTags = request.TagIds.Select(tagId => new QuestionTag { TagId = tagId }).ToList()
            };

            var createdQuestion = await _questionRepository.CreateAsync(newQuestion);
            return await GetQuestionByIdAsync(createdQuestion.Id);
        }

        public async Task<QuestionResponseDto> GetQuestionByIdAsync(int id)
        {
            var question = await _questionRepository.GetByIdAsync(id);
            if (question == null)
            {
                throw new Exception("Question not found.");
            }
            return MapToResponseDto(question);
        }

        public async Task<IEnumerable<QuestionResponseDto>> GetAllQuestionsAsync()
        {
            var questions = await _questionRepository.GetAllAsync();
            return questions.Select(MapToResponseDto);
        }

        public async Task<QuestionResponseDto> UpdateQuestionAsync(int id, QuestionRequestDto request)
        {
            var questionToUpdate = await _questionRepository.GetByIdAsync(id);
            if (questionToUpdate == null)
            {
                throw new Exception("Question not found.");
            }

            if (!request.Answers.Any(a => a.IsCorrect))
            {
                throw new Exception("A question must have at least one correct answer.");
            }

            questionToUpdate.Content = request.Content;
            questionToUpdate.Type = request.Type;

            questionToUpdate.Answers.Clear();
            foreach (var answerDto in request.Answers)
            {
                questionToUpdate.Answers.Add(new Answer { Content = answerDto.Content, IsCorrect = answerDto.IsCorrect });
            }

            questionToUpdate.QuestionTags.Clear();
            foreach (var tagId in request.TagIds)
            {
                questionToUpdate.QuestionTags.Add(new QuestionTag { TagId = tagId });
            }

            var updatedQuestion = await _questionRepository.UpdateAsync(questionToUpdate);
            return await GetQuestionByIdAsync(updatedQuestion.Id);
        }

        public async Task<bool> DeleteQuestionAsync(int id)
        {
            return await _questionRepository.DeleteAsync(id);
        }

        private QuestionResponseDto MapToResponseDto(Question question)
        {
            return new QuestionResponseDto
            {
                Id = question.Id,
                Content = question.Content,
                Type = question.Type,
                Answers = question.Answers.Select(a => new AnswerDto { Content = a.Content, IsCorrect = a.IsCorrect }).ToList(),
                Tags = question.QuestionTags.Select(qt => new TagDto { Id = qt.TagId, Name = qt.Tag.Name }).ToList()
            };
        }
    }
}
