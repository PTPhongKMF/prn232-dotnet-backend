namespace MathslideLearning.Data.Entities
{
    public class SlideTag
    {
        public int SlideId { get; set; }
        public virtual Slide Slide { get; set; }

        public int TagId { get; set; }
        public virtual Tag Tag { get; set; }
    }
}
