using MathslideLearning.Data.Entities;
using System.Threading.Tasks;

namespace MathslideLearning.Data.Interfaces
{
    public interface ISlidePageRepository
    {
        Task<SlidePage> GetByIdAsync(int pageId);
        Task<SlidePage> AddAsync(SlidePage newPage);
        Task UpdateAsync(SlidePage pageToUpdate);
        Task DeleteAsync(SlidePage pageToDelete);
    }
}
