using System.Collections.Generic;
using MathslideLearning.Data.Entities;

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
        public bool IsDeleted { get; set; } = false;

        public virtual ICollection<Slide> CreatedSlides { get; set; } = new List<Slide>();
        public virtual ICollection<Question> CreatedQuestions { get; set; } = new List<Question>();
        public virtual ICollection<Exam> CreatedExams { get; set; } = new List<Exam>();
        public virtual ICollection<Receipt> Receipts { get; set; } = new List<Receipt>();
        public virtual ICollection<UserExamHistory> ExamHistories { get; set; } = new List<UserExamHistory>();
    }
}

