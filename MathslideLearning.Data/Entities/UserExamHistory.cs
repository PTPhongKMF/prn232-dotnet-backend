using System;

namespace MathslideLearning.Data.Entities
{
    public class UserExamHistory
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ExamId { get; set; }
        public decimal Score { get; set; }
        public DateTime SubmittedAt { get; set; }
        public string? Content { get; set; } 

        public virtual User User { get; set; }
        public virtual Exam Exam { get; set; }
    }
}
