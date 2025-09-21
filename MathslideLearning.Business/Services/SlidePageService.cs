using MathslideLearning.Business.Interfaces;
using MathslideLearning.Data.Entities;
using MathslideLearning.Data.Interfaces;
using MathslideLearning.Models.SlideDtos;
using System;
using System.Threading.Tasks;

namespace MathslideLearning.Business.Services
{
    public class SlidePageService : ISlidePageService
    {
        private readonly ISlideRepository _slideRepository;
        private readonly ISlidePageRepository _slidePageRepository;

        public SlidePageService(ISlideRepository slideRepository, ISlidePageRepository slidePageRepository)
        {
            _slideRepository = slideRepository;
            _slidePageRepository = slidePageRepository;
        }

        public async Task<SlidePage> AddPageToSlideAsync(int slideId, int teacherId, SlidePageRequestDto pageDto)
        {
            var slide = await _slideRepository.GetSlideByIdAsync(slideId);
            if (slide == null) throw new Exception("Slide not found.");
            if (slide.TeacherId != teacherId) throw new Exception("You are not authorized to modify this slide.");

            var newPage = new SlidePage
            {
                SlideId = slideId,
                OrderNumber = pageDto.OrderNumber,
                Title = pageDto.Title,
                Content = pageDto.Content
            };

            return await _slidePageRepository.AddAsync(newPage);
        }

        public async Task<SlidePage> UpdateSlidePageAsync(int slideId, int pageId, int teacherId, SlidePageRequestDto pageDto)
        {
            var slide = await _slideRepository.GetSlideByIdAsync(slideId);
            if (slide == null) throw new Exception("Slide not found.");
            if (slide.TeacherId != teacherId) throw new Exception("You are not authorized to modify this slide.");

            var pageToUpdate = await _slidePageRepository.GetByIdAsync(pageId);
            if (pageToUpdate == null || pageToUpdate.SlideId != slideId) throw new Exception("Page not found in this slide.");

            pageToUpdate.OrderNumber = pageDto.OrderNumber;
            pageToUpdate.Title = pageToUpdate.Title;
            pageToUpdate.Content = pageDto.Content;

            await _slidePageRepository.UpdateAsync(pageToUpdate);
            return pageToUpdate;
        }

        public async Task DeleteSlidePageAsync(int slideId, int pageId, int teacherId)
        {
            var slide = await _slideRepository.GetSlideByIdAsync(slideId);
            if (slide == null) throw new Exception("Slide not found.");
            if (slide.TeacherId != teacherId) throw new Exception("You are not authorized to modify this slide.");

            var pageToDelete = await _slidePageRepository.GetByIdAsync(pageId);
            if (pageToDelete == null || pageToDelete.SlideId != slideId) throw new Exception("Page not found in this slide.");

            await _slidePageRepository.DeleteAsync(pageToDelete);
        }
    }
}
