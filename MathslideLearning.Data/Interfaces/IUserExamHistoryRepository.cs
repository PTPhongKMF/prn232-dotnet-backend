using MathslideLearning.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MathslideLearning.Data.Interfaces
{
    public interface IUserExamHistoryRepository
    {
        Task<UserExamHistory> CreateAsync(UserExamHistory history);
        Task<IEnumerable<UserExamHistory>> GetByUserIdAsync(int userId);
    }
}
