using MathslideLearning.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MathslideLearning.Data.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUserByEmailAsync(string email);
        Task<User> GetUserByIdAsync(int id);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> CreateUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task<bool> SoftDeleteUserAsync(int id);
    }
}

