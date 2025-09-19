using System.Collections.Generic;

namespace MathslideLearning.Data.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public int? Grade { get; set; }
        public virtual ICollection<Slide> CreatedSlides { get; set; } = new List<Slide>();
        public virtual ICollection<PurchasedSlide> PurchasedSlides { get; set; } = new List<PurchasedSlide>();
    }
}
