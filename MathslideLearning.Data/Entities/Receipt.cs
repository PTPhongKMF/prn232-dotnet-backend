using System;
using System.Collections.Generic;

namespace MathslideLearning.Data.Entities
{
    public class Receipt
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int PaymentMethodId { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual User User { get; set; }
        public virtual PaymentMethod PaymentMethod { get; set; }
        public virtual ICollection<ReceiptDetail> ReceiptDetails { get; set; } = new List<ReceiptDetail>();
    }
}
