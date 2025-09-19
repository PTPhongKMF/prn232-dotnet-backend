using System.ComponentModel.DataAnnotations;

namespace MathslideLearning.Models.SlideDtos
{
    public class SlidePageCreateDto
    {
        [Required]
        public int OrderNumber { get; set; }

        public string? Title { get; set; }

        public string? Content { get; set; }
    }
}
