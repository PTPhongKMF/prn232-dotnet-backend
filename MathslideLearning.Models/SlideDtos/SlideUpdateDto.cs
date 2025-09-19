using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathslideLearning.Models.SlideDtos
{
    public class SlideUpdateDto
    {
        [Required]
        [MinLength(3, ErrorMessage = "Title must be at least 3 characters long.")]
        public string Title { get; set; }

        public string? Topic { get; set; }

        public string? ContentType { get; set; }

        [Required]
        [Range(0, 1000000, ErrorMessage = "Price must be a positive number.")]
        public decimal Price { get; set; }

        [Range(1, 12, ErrorMessage = "Grade must be between 1 and 12.")]
        public int? Grade { get; set; }

        public bool IsPublished { get; set; }

        [Required]
        public List<SlidePageCreateDto> SlidePages { get; set; }
    }
}
