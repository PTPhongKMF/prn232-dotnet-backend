using MathslideLearning.Models.PagnitionDtos;

namespace MathslideLearning.Models.TagDtos
{
    public class FilteredPagedTagRequestDto : PaginationDto
    {
        public string? SearchTerm { get; set; }
    }
}
