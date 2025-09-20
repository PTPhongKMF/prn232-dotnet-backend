using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MathslideLearning.Models.QuestionDtos
{
    public class QuestionRequestDto
    {
        [Required]
        public string Content { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        [MinLength(2, ErrorMessage = "A question must have at least two answers.")]
        public List<AnswerDto> Answers { get; set; }

        public List<int> TagIds { get; set; } = new List<int>();
    }
}
