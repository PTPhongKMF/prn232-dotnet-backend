using System;
using System.Collections.Generic;
using MathslideLearning.Models.PagnitionDtos;

namespace MathslideLearning.Models.QuestionDtos
{
    public class FilteredPagedQuestionRequestDto : PaginationDto
    {
        public string? SearchTerm { get; set; }
        public List<int>? TagIds { get; set; }
        public bool SortByDateDescending { get; set; } = true;
    }
}
