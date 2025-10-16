using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathslideLearning.Models.ExamDtos
{
    public class AddExistingQuestionsRequestDto
    {
        public List<int> QuestionIds { get; set; } = new();
    }
}
