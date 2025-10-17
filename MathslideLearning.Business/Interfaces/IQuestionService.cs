using MathslideLearning.Models.PagnitionDtos;
using MathslideLearning.Models.QuestionDtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MathslideLearning.Business.Interfaces
{
    public interface IQuestionService
    {
        Task<QuestionResponseDto> CreateQuestionAsync(QuestionRequestDto request, int teacherId);
        Task<QuestionResponseDto> GetQuestionByIdAsync(int id);
        Task<IEnumerable<QuestionResponseDto>> GetAllQuestionsAsync();
        Task<QuestionResponseDto> UpdateQuestionAsync(int id, QuestionRequestDto request);
        Task<bool> DeleteQuestionAsync(int id);
        Task<PagedResult<QuestionResponseDto>> GetFilteredPagedQuestionsAsync(FilteredPagedQuestionRequestDto request);
    }
}
