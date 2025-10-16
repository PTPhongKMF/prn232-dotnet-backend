using MathslideLearning.Models.ExamDtos;
using MathslideLearning.Models.QuestionDtos;
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

        Task<ExamResultDto> SubmitExamAsync(int studentId, int examId, ExamSubmissionDto submission);
        Task<IEnumerable<UserExamHistoryDto>> GetExamHistoryForStudentAsync(int studentId);
        Task<bool> AddExistingQuestionsToExamAsync(int examId, List<int> questionIds);
        Task<bool> RemoveQuestionFromExamAsync(int examId, int questionId);

    }
}
