using MathslideLearning.Data.DbContext;
using MathslideLearning.Data.Entities;
using MathslideLearning.Data.Interfaces;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MathslideLearning.Data.Repositories
{
    public class QuestionRepository : IQuestionRepository
    {
        private readonly MathslideLearningDbContext _context;

        public QuestionRepository(MathslideLearningDbContext context)
        {
            _context = context;
        }

        public async Task<Question> GetByIdAsync(int id)
        {
            return await _context.Questions
                .Include(q => q.Answers)
                .Include(q => q.QuestionTags)
                    .ThenInclude(qt => qt.Tag)
                .Include(q => q.Teacher)
                .Include(q => q.ExamQuestions)
                .FirstOrDefaultAsync(q => q.Id == id);
        }

        public async Task<IEnumerable<Question>> GetAllAsync()
        {
            return await _context.Questions
                .Include(q => q.Answers)
                .Include(q => q.QuestionTags)
                    .ThenInclude(qt => qt.Tag)
                .Include(q => q.Teacher)
                .Include(q => q.ExamQuestions)
                .ToListAsync();
        }

        public async Task<Question> CreateAsync(Question question)
        {
            await _context.Questions.AddAsync(question);
            await _context.SaveChangesAsync();
            return question;
        }

        public async Task<Question> UpdateAsync(Question question)
        {
            _context.Questions.Update(question);
            await _context.SaveChangesAsync();
            return question;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var question = await GetByIdAsync(id);
            if (question == null) return false;

            if (question.ExamQuestions.Any())
            {
                throw new InvalidOperationException("Cannot delete question because it is being used in one or more exams.");
            }

            _context.Questions.Remove(question);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<(IEnumerable<Question> items, int totalCount)> GetFilteredQuestionsAsync(
            string? searchTerm,
            IEnumerable<int>? tagIds,
            bool sortByDateDescending,
            int skip,
            int take,
            DateTime? from = null,
            DateTime? to = null)
        {
            var query = _context.Questions
                .Include(q => q.Answers)
                .Include(q => q.QuestionTags)
                    .ThenInclude(qt => qt.Tag)
                .Include(q => q.Teacher)
                .Include(q => q.ExamQuestions)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(q => q.Content.Contains(searchTerm));
            }

            if (tagIds != null && tagIds.Any())
            {
                query = query.Where(q => q.QuestionTags.Any(qt => tagIds.Contains(qt.TagId)));
            }
            
            if (from.HasValue)
            {
                query = query.Where(q => q.CreatedAt >= from.Value);
            }
            
            if (to.HasValue)
            {
                query = query.Where(q => q.CreatedAt <= to.Value);
            }

            query = sortByDateDescending
                ? query.OrderByDescending(q => q.CreatedAt)
                : query.OrderBy(q => q.CreatedAt);

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            return (items, totalCount);
        }
    }
}
