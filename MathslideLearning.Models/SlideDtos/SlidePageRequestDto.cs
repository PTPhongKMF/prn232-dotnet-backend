using System.ComponentModel.DataAnnotations;

namespace MathslideLearning.Models.SlideDtos
{
    public class SlidePageRequestDto
    {
        [Required]
        public int OrderNumber { get; set; }

        [MaxLength(255)]
        public string? Title { get; set; }
        public string? Content { get; set; }
    }
}
