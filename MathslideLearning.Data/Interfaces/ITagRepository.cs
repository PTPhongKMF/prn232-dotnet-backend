using MathslideLearning.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MathslideLearning.Data.Interfaces
{
    public interface ITagRepository
    {
        Task<IEnumerable<Tag>> GetAllAsync();
        Task<Tag> GetByIdAsync(int id);
        Task<Tag> CreateAsync(Tag tag);
        Task<Tag> UpdateAsync(Tag tag);
        Task<bool> DeleteAsync(int id);
        Task<Tag> GetByNameAsync(string name);
        Task<(IEnumerable<Tag> items, int totalCount)> GetFilteredTagsAsync(
            string? searchTerm,
            int skip,
            int take);
    }
}