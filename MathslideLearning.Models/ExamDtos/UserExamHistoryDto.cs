using System;

namespace MathslideLearning.Models.ExamDtos
{
    public class UserExamHistoryDto
    {
        public int Id { get; set; }
        public int ExamId { get; set; }
        public string ExamName { get; set; }
        public decimal Score { get; set; }
        public DateTime SubmittedAt { get; set; }
        public string Content { get; set; }
    }
}
