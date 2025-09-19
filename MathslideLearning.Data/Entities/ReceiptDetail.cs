namespace MathslideLearning.Data.Entities
{
    public class ReceiptDetail
    {
        public int ReceiptId { get; set; }
        public virtual Receipt Receipt { get; set; }

        public int SlideId { get; set; }
        public virtual Slide Slide { get; set; }
    }
}
