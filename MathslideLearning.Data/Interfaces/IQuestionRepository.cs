using MathslideLearning.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MathslideLearning.Data.Interfaces
{
    public interface IQuestionRepository
    {
        Task<Question> GetByIdAsync(int id);
        Task<IEnumerable<Question>> GetAllAsync();
        Task<Question> CreateAsync(Question question);
        Task<Question> UpdateAsync(Question question);
        Task<bool> DeleteAsync(int id);
        Task<(IEnumerable<Question> items, int totalCount)> GetFilteredQuestionsAsync(
            string? searchTerm,
            IEnumerable<int>? tagIds,
            bool sortByDateDescending,
            int skip,
            int take,
            DateTime? from = null,
            DateTime? to = null);
    }
}
