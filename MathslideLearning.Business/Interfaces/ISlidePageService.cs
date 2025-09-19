using MathslideLearning.Common.Models;
using MathslideLearning.Data.Entities;
using System.Threading.Tasks;

namespace MathslideLearning.Business.Interfaces
{
    public interface ISlidePageService
    {
        Task<SlidePage> AddPageToSlideAsync(int slideId, int teacherId, SlidePageRequestDto pageDto);
        Task<SlidePage> UpdateSlidePageAsync(int slideId, int pageId, int teacherId, SlidePageRequestDto pageDto);
        Task DeleteSlidePageAsync(int slideId, int pageId, int teacherId);
    }
}
