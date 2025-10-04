using MathslideLearning.Data.DbContext;
using MathslideLearning.Data.Entities;
using MathslideLearning.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MathslideLearning.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly MathslideLearningDbContext _context;

        public UserRepository(MathslideLearningDbContext context)
        {
            _context = context;
        }

        // --- New Method to Get All Users ---
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            // AsNoTracking is a performance optimization for read-only queries.
            return await _context.Users.AsNoTracking().ToListAsync();
        }

        // --- Existing Methods ---
        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User> CreateUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> SoftDeleteUserAsync(int id)
        {
            var user = await GetUserByIdAsync(id);
            if (user == null) return false;
            
            user.IsDeleted = true;
            user.Email = string.Empty;
            
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

