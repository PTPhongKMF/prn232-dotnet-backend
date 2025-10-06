using MathslideLearning.Models.PagnitionDtos;
using MathslideLearning.Models.TagDtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MathslideLearning.Business.Interfaces
{
    public interface ITagService
    {
        Task<IEnumerable<TagDto>> GetAllTagsAsync();
        Task<TagDto> GetTagByIdAsync(int id);
        Task<TagDto> CreateTagAsync(TagRequestDto request);
        Task<TagDto> UpdateTagAsync(int id, TagRequestDto request);
        Task<bool> DeleteTagAsync(int id);
        Task<PagedResult<TagDto>> GetFilteredPagedTagsAsync(FilteredPagedTagRequestDto request);
    }
}
