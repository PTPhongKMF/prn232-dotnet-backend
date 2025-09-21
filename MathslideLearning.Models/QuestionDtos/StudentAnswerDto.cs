using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MathslideLearning.Models.QuestionDtos
{

    public class StudentAnswerDto
    {
        [Required]
        public int QuestionId { get; set; }

        [Required]
        public int AnswerId { get; set; }
    }

    public class ExamSubmissionDto
    {
        [Required]
        [MinLength(1)]
        public List<StudentAnswerDto> Answers { get; set; }
    }
}
