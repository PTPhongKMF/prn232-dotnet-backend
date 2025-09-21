using System;

namespace MathslideLearning.Models.ExamDtos
{
    public class ExamResultDto
    {
        public int ExamId { get; set; }
        public string ExamName { get; set; }
        public int Score { get; set; }
        public int TotalQuestions { get; set; }
        public DateTime SubmittedAt { get; set; }
    }
}
