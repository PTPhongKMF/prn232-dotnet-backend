using MathslideLearning.Business.Interfaces;
using MathslideLearning.Models.TagDtos;
using MathslideLearning.Data.Entities;
using MathslideLearning.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MathslideLearning.Business.Services
{
    public class TagService : ITagService
    {
        private readonly ITagRepository _tagRepository;

        public TagService(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        public async Task<IEnumerable<TagDto>> GetAllTagsAsync()
        {
            var tags = await _tagRepository.GetAllAsync();
            return tags.Select(t => new TagDto { Id = t.Id, Name = t.Name });
        }

        public async Task<TagDto> GetTagByIdAsync(int id)
        {
            var tag = await _tagRepository.GetByIdAsync(id);
            if (tag == null)
            {
                throw new Exception("Tag not found.");
            }
            return new TagDto { Id = tag.Id, Name = tag.Name };
        }

        public async Task<TagDto> CreateTagAsync(TagRequestDto request)
        {
            var existingTag = await _tagRepository.GetByNameAsync(request.Name);
            if (existingTag != null)
            {
                throw new Exception($"A tag with the name '{request.Name}' already exists.");
            }

            var newTag = new Tag { Name = request.Name };
            var createdTag = await _tagRepository.CreateAsync(newTag);

            return new TagDto { Id = createdTag.Id, Name = createdTag.Name };
        }

        public async Task<TagDto> UpdateTagAsync(int id, TagRequestDto request)
        {
            var tagToUpdate = await _tagRepository.GetByIdAsync(id);
            if (tagToUpdate == null)
            {
                throw new Exception("Tag not found.");
            }

            var existingTag = await _tagRepository.GetByNameAsync(request.Name);
            if (existingTag != null && existingTag.Id != id)
            {
                throw new Exception($"Another tag with the name '{request.Name}' already exists.");
            }

            tagToUpdate.Name = request.Name;
            var updatedTag = await _tagRepository.UpdateAsync(tagToUpdate);

            return new TagDto { Id = updatedTag.Id, Name = updatedTag.Name };
        }

        public async Task<bool> DeleteTagAsync(int id)
        {
            var tag = await _tagRepository.GetByIdAsync(id);
            if (tag == null)
            {
                throw new Exception("Tag not found.");
            }

            return await _tagRepository.DeleteAsync(id);
        }
    }
}
