namespace MathslideLearning.Data.Entities
{
    public class SlidePage
    {
        public int Id { get; set; }
        public int SlideId { get; set; }
        public int OrderNumber { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public virtual Slide Slide { get; set; }
    }
}
