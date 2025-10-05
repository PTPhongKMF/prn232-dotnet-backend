using System;
using System.Collections.Generic;
using MathslideLearning.Models.PagnitionDtos;

namespace MathslideLearning.Models.QuestionDtos
{
    public class FilteredPagedQuestionRequestDto : PaginationDto
    {
        public string? SearchTerm { get; set; }
        public string? TagIds { get; set; }
        public bool SortByDateDescending { get; set; } = true;
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }

        public List<int> GetTagIdsList()
        {
            if (string.IsNullOrWhiteSpace(TagIds))
                return new List<int>();

            return TagIds
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(id => int.TryParse(id.Trim(), out var parsedId) ? parsedId : (int?)null)
                .Where(id => id.HasValue)                            
                .Select(id => id!.Value)
                .ToList();
        }
    }
}
