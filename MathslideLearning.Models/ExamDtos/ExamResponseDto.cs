using MathslideLearning.Models.QuestionDtos;
using System.Collections.Generic;

namespace MathslideLearning.Models.ExamDtos
{
    public class ExamResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public int TeacherId { get; set; }
        public string TeacherName { get; set; }
        public int QuestionsCount { get; set; }
        public List<QuestionResponseDto> Questions { get; set; }
    }
}