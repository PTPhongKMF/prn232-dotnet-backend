using MathslideLearning.Data.DbContext;
using MathslideLearning.Data.Entities;
using MathslideLearning.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MathslideLearning.Data.Repositories
{
    public class SlideRepository : ISlideRepository
    {
        private readonly MathslideLearningDbContext _context;

        public SlideRepository(MathslideLearningDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Slide>> GetAllPublicSlidesAsync()
        {
            return await _context.Slides
                .Include(s => s.Teacher)
                .Include(s => s.SlidePages)
                .Where(s => s.IsPublished)
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<Slide> GetSlideByIdAsync(int slideId)
        {
            return await _context.Slides
                .Include(s => s.SlidePages)
                .FirstOrDefaultAsync(s => s.Id == slideId);
        }

        public async Task<IEnumerable<Slide>> GetSlidesByTeacherIdAsync(int teacherId)
        {
            return await _context.Slides
                .Where(s => s.TeacherId == teacherId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Slide> UpdateSlideAsync(Slide slide)
        {
            _context.Slides.Update(slide);
            await _context.SaveChangesAsync();
            return slide;
        }

        public async Task<bool> DeleteSlideAsync(int slideId)
        {
            var slide = await GetSlideByIdAsync(slideId);
            if (slide == null)
            {
                return false;
            }
            _context.Slides.Remove(slide);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Slide> CreateSlideAsync(Slide slide)
        {
            _context.Slides.Add(slide);
            await _context.SaveChangesAsync();
            return slide;
        }
    }
}

