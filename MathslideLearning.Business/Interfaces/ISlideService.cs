using MathslideLearning.Data.Entities;
using MathslideLearning.Models.SlideDtos;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MathslideLearning.Business.Interfaces
{
    public interface ISlideService
    {
        Task<Slide> CreateSlideAsync(SlideCreateDto slideDto, int teacherId, IFormFile file);
        Task<Slide> UpdateSlideAsync(int slideId, SlideUpdateDto slideDto, int teacherId, IFormFile? file);
        Task<bool> DeleteSlideAsync(int slideId, int teacherId);
        Task<IEnumerable<Slide>> GetSlidesByTeacherIdAsync(int teacherId);
        Task<Slide> UpdateSlideStatusAsync(int slideId, int teacherId, bool isPublished);
        Task<IEnumerable<Slide>> GetAllPublicSlidesAsync();
    }
}