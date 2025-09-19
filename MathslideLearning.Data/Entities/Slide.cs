using System;
using System.Collections.Generic;

namespace MathslideLearning.Data.Entities
{
    public class Slide
    {
        public int Id { get; set; }
        public int TeacherId { get; set; }
        public string Title { get; set; }
        public string? Topic { get; set; }
        public string? ContentType { get; set; }
        public string? FileUrl { get; set; }
        public decimal Price { get; set; }
        public int? Grade { get; set; }
        public bool IsPublished { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual User Teacher { get; set; }
        public virtual ICollection<SlidePage> SlidePages { get; set; } = new List<SlidePage>();
        public virtual ICollection<SlideTag> SlideTags { get; set; } = new List<SlideTag>();
        public virtual ICollection<ReceiptDetail> ReceiptDetails { get; set; } = new List<ReceiptDetail>();
    }
}