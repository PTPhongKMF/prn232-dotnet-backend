using System.Collections.Generic;

namespace MathslideLearning.Data.Entities
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<SlideTag> SlideTags { get; set; } = new List<SlideTag>();
        public virtual ICollection<QuestionTag> QuestionTags { get; set; } = new List<QuestionTag>();
    }
}