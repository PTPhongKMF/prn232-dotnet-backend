using MathslideLearning.Data.DbContext;
using MathslideLearning.Data.Entities;
using MathslideLearning.Data.Interfaces;
using System.Threading.Tasks;

namespace MathslideLearning.Data.Repositories
{
    public class SlidePageRepository : ISlidePageRepository
    {
        private readonly MathslideLearningDbContext _context;

        public SlidePageRepository(MathslideLearningDbContext context)
        {
            _context = context;
        }

        public async Task<SlidePage> GetByIdAsync(int pageId)
        {
            return await _context.SlidePages.FindAsync(pageId);
        }

        public async Task<SlidePage> AddAsync(SlidePage newPage)
        {
            await _context.SlidePages.AddAsync(newPage);
            await _context.SaveChangesAsync();
            return newPage;
        }

        public async Task UpdateAsync(SlidePage pageToUpdate)
        {
            _context.SlidePages.Update(pageToUpdate);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(SlidePage pageToDelete)
        {
            _context.SlidePages.Remove(pageToDelete);
            await _context.SaveChangesAsync();
        }
    }
}
