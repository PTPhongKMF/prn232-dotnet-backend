using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MathslideLearning.Models.ExamDtos
{
    public class ExamRequestDto
    {
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public required string Name { get; set; }

        public required string Content { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "An exam must have at least one question.")]
        public required List<int>? QuestionIds { get; set; }
    }
}