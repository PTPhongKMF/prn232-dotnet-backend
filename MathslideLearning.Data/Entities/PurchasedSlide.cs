using System;

namespace MathslideLearning.Data.Entities
{
    public class PurchasedSlide
    {
        public int UserId { get; set; }
        public virtual User User { get; set; }

        public int SlideId { get; set; }
        public virtual Slide Slide { get; set; }

        public DateTime PurchasedAt { get; set; }
    }
}
