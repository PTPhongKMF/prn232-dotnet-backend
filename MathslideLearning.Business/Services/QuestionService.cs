using AutoMapper;
using MathslideLearning.Business.Interfaces;
using MathslideLearning.Data.Entities;
using MathslideLearning.Data.Interfaces;
using MathslideLearning.Models.PagnitionDtos;
using MathslideLearning.Models.QuestionDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MathslideLearning.Business.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly IMapper _mapper;

        public QuestionService(IQuestionRepository questionRepository, IMapper mapper)
        {
            _questionRepository = questionRepository;
            _mapper = mapper;
        }

        public async Task<QuestionResponseDto> CreateQuestionAsync(QuestionRequestDto request, int teacherId)
        {
            if (!request.Answers.Any(a => a.IsCorrect))
            {
                throw new Exception("A question must have at least one correct answer.");
            }

            var newQuestion = new Question
            {
                Content = request.Content,
                Type = request.Type,
                TeacherId = teacherId,
                Answers = request.Answers.Select(a => new Answer { Content = a.Content, IsCorrect = a.IsCorrect }).ToList(),
                QuestionTags = request.TagIds.Select(tagId => new QuestionTag { TagId = tagId }).ToList()
            };

            var createdQuestion = await _questionRepository.CreateAsync(newQuestion);
            return _mapper.Map<QuestionResponseDto>(createdQuestion);
        }

        public async Task<QuestionResponseDto> GetQuestionByIdAsync(int id)
        {
            var question = await _questionRepository.GetByIdAsync(id);
            if (question == null)
            {
                throw new Exception("Question not found.");
            }
            return _mapper.Map<QuestionResponseDto>(question);
        }

        public async Task<IEnumerable<QuestionResponseDto>> GetAllQuestionsAsync()
        {
            var questions = await _questionRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<QuestionResponseDto>>(questions);
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
            return _mapper.Map<QuestionResponseDto>(updatedQuestion);
        }

        public async Task<bool> DeleteQuestionAsync(int id)
        {
            return await _questionRepository.DeleteAsync(id);
        }

        public async Task<PagedResult<QuestionResponseDto>> GetFilteredPagedQuestionsAsync(FilteredPagedQuestionRequestDto request)
        {
            var skip = (request.PageNumber - 1) * request.PageSize;
            var (items, totalCount) = await _questionRepository.GetFilteredQuestionsAsync(
                request.SearchTerm,
                request.GetTagIdsList(),
                request.SortByDateDescending,
                skip,
                request.PageSize,
                request.From,
                request.To);
            
            var totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);
            
            return new PagedResult<QuestionResponseDto>
            {
                Results = _mapper.Map<IEnumerable<QuestionResponseDto>>(items),
                Pagnition = new PaginationDto
                {
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize,
                    TotalPages = totalPages
                }
            };
        }
    }
}
