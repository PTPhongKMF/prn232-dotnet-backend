using MathslideLearning.Data.DbContext;
using MathslideLearning.Data.Entities;
using MathslideLearning.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MathslideLearning.Data.Repositories
{
    public class UserExamHistoryRepository : IUserExamHistoryRepository
    {
        private readonly MathslideLearningDbContext _context;

        public UserExamHistoryRepository(MathslideLearningDbContext context)
        {
            _context = context;
        }

        public async Task<UserExamHistory> CreateAsync(UserExamHistory history)
        {
            await _context.UserExamHistories.AddAsync(history);
            await _context.SaveChangesAsync();
            return history;
        }

        public async Task<IEnumerable<UserExamHistory>> GetByUserIdAsync(int userId)
        {
            return await _context.UserExamHistories
                .Include(h => h.Exam)
                .Where(h => h.UserId == userId)
                .OrderByDescending(h => h.SubmittedAt)
                .ToListAsync();
        }
    }
}
