using MathslideLearning.Models.TagDtos;
using System.Collections.Generic;

namespace MathslideLearning.Models.QuestionDtos
{
    public class QuestionResponseDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string Type { get; set; }
        public int TeacherId { get; set; }
        public string TeacherName { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool Deleteable { get; set; }
        public List<AnswerDto> Answers { get; set; }
        public List<TagDto> Tags { get; set; }
    }
}
