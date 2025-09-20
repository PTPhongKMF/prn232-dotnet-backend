using MathslideLearning.Models.ExamDtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MathslideLearning.Business.Interfaces
{
    public interface IExamService
    {
        Task<ExamResponseDto> CreateExamAsync(int teacherId, ExamRequestDto request);
        Task<ExamResponseDto> GetExamByIdAsync(int examId);
        Task<IEnumerable<ExamResponseDto>> GetExamsByTeacherAsync(int teacherId);
        Task<ExamResponseDto> UpdateExamAsync(int examId, int teacherId, ExamRequestDto request);
        Task<bool> DeleteExamAsync(int examId, int teacherId);
    }
}
