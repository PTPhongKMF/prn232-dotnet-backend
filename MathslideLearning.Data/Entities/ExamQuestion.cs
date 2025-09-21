namespace MathslideLearning.Data.Entities
{
    public class ExamQuestion
    {
        public int ExamId { get; set; }
        public virtual Exam Exam { get; set; }

        public int QuestionId { get; set; }
        public virtual Question Question { get; set; }
    }
}
