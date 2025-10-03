using System.ComponentModel.DataAnnotations;

namespace MathslideLearning.Models.SlideDtos
{
    public class SlideStatusUpdateDto
    {
        [Required]
        public bool IsPublished { get; set; }
    }
}