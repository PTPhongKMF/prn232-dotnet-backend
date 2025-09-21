using System;

namespace MathslideLearning.Models.PaymentDtos
{
    public class TeacherSaleDto
    {
        public int ReceiptId { get; set; }
        public string SlideTitle { get; set; }
        public decimal SalePrice { get; set; }
        public string StudentName { get; set; }
        public DateTime PurchaseDate { get; set; }
    }
}
