using MathslideLearning.Common.Models;
using MathslideLearning.Data.Entities;
using MathslideLearning.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MathslideLearning.Business.Interfaces
{
    public interface ISlideService
    {
        Task<Slide> CreateSlideAsync(SlideCreateDto slideDto, int teacherId);
        Task<Slide> UpdateSlideAsync(int slideId, SlideUpdateDto slideDto, int teacherId);
        Task<bool> DeleteSlideAsync(int slideId, int teacherId);
        Task<IEnumerable<Slide>> GetSlidesByTeacherIdAsync(int teacherId);
    }
}

