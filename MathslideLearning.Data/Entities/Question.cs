using System.Collections.Generic;
using MathslideLearning.Data.Entities;

namespace MathslideLearning.Data.Entities
{
    public class Question
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string Type { get; set; }
        public int TeacherId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public virtual User Teacher { get; set; }
        public virtual ICollection<Answer> Answers { get; set; } = new List<Answer>();
        public virtual ICollection<ExamQuestion> ExamQuestions { get; set; } = new List<ExamQuestion>();
        public virtual ICollection<QuestionTag> QuestionTags { get; set; } = new List<QuestionTag>();
    }
}

