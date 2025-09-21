using System.ComponentModel.DataAnnotations;

namespace MathslideLearning.Models.TagDtos
{
    public class TagRequestDto
    {
        [Required(ErrorMessage = "Tag name is required.")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Tag name must be between 1 and 100 characters.")]
        public string Name { get; set; }
    }
}
