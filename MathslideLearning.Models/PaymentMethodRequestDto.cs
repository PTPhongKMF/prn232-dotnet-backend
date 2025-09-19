using System.ComponentModel.DataAnnotations;

namespace MathslideLearning.Common.Models
{
    public class PaymentMethodRequestDto
    {
        [Required(ErrorMessage = "Payment method name is required.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 50 characters.")]
        public string Name { get; set; }
    }
}
