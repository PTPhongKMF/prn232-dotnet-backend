using MathslideLearning.Data.DbContext;
using MathslideLearning.Data.Entities;
using MathslideLearning.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MathslideLearning.Data.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly MathslideLearningDbContext _context;

        public TagRepository(MathslideLearningDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Tag>> GetAllAsync()
        {
            return await _context.Tags.ToListAsync();
        }

        public async Task<Tag> GetByIdAsync(int id)
        {
            return await _context.Tags.FindAsync(id);
        }

        public async Task<Tag> GetByNameAsync(string name)
        {
            return await _context.Tags.FirstOrDefaultAsync(t => t.Name.ToLower() == name.ToLower());
        }

        public async Task<Tag> CreateAsync(Tag tag)
        {
            await _context.Tags.AddAsync(tag);
            await _context.SaveChangesAsync();
            return tag;
        }

        public async Task<Tag> UpdateAsync(Tag tag)
        {
            _context.Tags.Update(tag);
            await _context.SaveChangesAsync();
            return tag;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var tag = await GetByIdAsync(id);
            if (tag == null)
            {
                return false;
            }

            _context.Tags.Remove(tag);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<(IEnumerable<Tag> items, int totalCount)> GetFilteredTagsAsync(
            string? searchTerm, 
            int skip, 
            int take)
        {
            var query = _context.Tags.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(t => t.Name.Contains(searchTerm));
            }

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderBy(t => t.Name)
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            return (items, totalCount);
        }
    }
}
