using System;
using System.Collections.Generic;
using MathslideLearning.Data.Entities;

namespace MathslideLearning.Data.Entities
{
    public class Exam
    {
        public int Id { get; set; }
        public int TeacherId { get; set; }
        public string Name { get; set; }
        public string? Content { get; set; }

        public virtual User Teacher { get; set; }

        public virtual ICollection<ExamQuestion> ExamQuestions { get; set; } = new List<ExamQuestion>();
    }
}

