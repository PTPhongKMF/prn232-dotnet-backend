using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MathslideLearning.Models.PaymentDtos
{
    public class PurchaseRequestDto
    {
        [Required]
        [MinLength(1, ErrorMessage = "You must select at least one slide to purchase.")]
        public List<int> SlideIds { get; set; }

        [Required(ErrorMessage = "A payment method is required.")]
        public int PaymentMethodId { get; set; }
    }
}

