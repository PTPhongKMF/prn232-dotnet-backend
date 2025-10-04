using MathslideLearning.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MathslideLearning.Data.Interfaces
{
    public interface ISlideRepository
    {
        Task<Slide> CreateSlideAsync(Slide slide);
        Task<Slide> GetSlideByIdAsync(int slideId);
        Task<IEnumerable<Slide>> GetSlidesByTeacherIdAsync(int teacherId);
        Task<Slide> UpdateSlideAsync(Slide slide);
        Task<bool> DeleteSlideAsync(int slideId);
        Task<IEnumerable<Slide>> GetAllPublicSlidesAsync();
    }
}

