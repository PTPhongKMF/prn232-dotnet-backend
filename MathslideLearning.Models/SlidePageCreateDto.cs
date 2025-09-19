using System.ComponentModel.DataAnnotations;

namespace MathslideLearning.Common.Models
{
    public class SlidePageCreateDto
    {
        [Required]
        public int OrderNumber { get; set; }

        public string? Title { get; set; }

        public string? Content { get; set; }
    }
}
