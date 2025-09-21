using System.Collections.Generic;

namespace MathslideLearning.Data.Entities
{
    public class PaymentMethod
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Receipt> Receipts { get; set; } = new List<Receipt>();
    }
}
