using MathslideLearning.Business.Interfaces;
using MathslideLearning.Common.Models;
using MathslideLearning.Data.Entities;
using MathslideLearning.Data.Interfaces;
using MathslideLearning.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MathslideLearning.Business.Services
{
    public class SlideService : ISlideService
    {
        private readonly ISlideRepository _slideRepository;

        public SlideService(ISlideRepository slideRepository)
        {
            _slideRepository = slideRepository;
        }
        public async Task<Slide> UpdateSlideAsync(int slideId, SlideUpdateDto slideDto, int teacherId)
        {
            var slideToUpdate = await _slideRepository.GetSlideByIdAsync(slideId);
            if (slideToUpdate == null)
            {
                throw new Exception("Slide not found.");
            }

            if (slideToUpdate.TeacherId != teacherId)
            {
                throw new UnauthorizedAccessException("You are not authorized to edit this slide.");
            }

            slideToUpdate.Title = slideDto.Title;
            slideToUpdate.Topic = slideDto.Topic;
            slideToUpdate.ContentType = slideDto.ContentType;
            slideToUpdate.Price = slideDto.Price;
            slideToUpdate.Grade = slideDto.Grade;
            slideToUpdate.IsPublished = slideDto.IsPublished;

            slideToUpdate.SlidePages.Clear();
            foreach (var pageDto in slideDto.SlidePages)
            {
                slideToUpdate.SlidePages.Add(new SlidePage
                {
                    OrderNumber = pageDto.OrderNumber,
                    Title = pageDto.Title,
                    Content = pageDto.Content
                });
            }

            return await _slideRepository.UpdateSlideAsync(slideToUpdate);
        }

        public async Task<bool> DeleteSlideAsync(int slideId, int teacherId)
        {
            var slideToDelete = await _slideRepository.GetSlideByIdAsync(slideId);
            if (slideToDelete == null)
            {
                throw new Exception("Slide not found.");
            }

            if (slideToDelete.TeacherId != teacherId)
            {
                throw new UnauthorizedAccessException("You are not authorized to delete this slide.");
            }

            return await _slideRepository.DeleteSlideAsync(slideId);
        }

        public async Task<IEnumerable<Slide>> GetSlidesByTeacherIdAsync(int teacherId)
        {
            return await _slideRepository.GetSlidesByTeacherIdAsync(teacherId);
        }
        public async Task<Slide> CreateSlideAsync(SlideCreateDto slideDto, int teacherId)
        {
            var newSlide = new Slide
            {
                TeacherId = teacherId,
                Title = slideDto.Title,
                Topic = slideDto.Topic,
                ContentType = slideDto.ContentType,
                Price = slideDto.Price,
                Grade = slideDto.Grade,
                IsPublished = slideDto.IsPublished,
                CreatedAt = DateTime.UtcNow,
                SlidePages = slideDto.SlidePages.Select(p => new SlidePage
                {
                    OrderNumber = p.OrderNumber,
                    Title = p.Title,
                    Content = p.Content
                }).ToList()
            };

            return await _slideRepository.CreateSlideAsync(newSlide);
        }
    }
}
