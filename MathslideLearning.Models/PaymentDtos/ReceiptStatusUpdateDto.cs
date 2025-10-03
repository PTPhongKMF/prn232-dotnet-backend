using System.ComponentModel.DataAnnotations;

namespace MathslideLearning.Models.PaymentDtos
{
    public class ReceiptStatusUpdateDto
    {
        [Required]
        [RegularExpression("^(Paid|Failed)$", ErrorMessage = "Status must be 'Paid' or 'Failed'.")]
        public string Status { get; set; }
    }
}