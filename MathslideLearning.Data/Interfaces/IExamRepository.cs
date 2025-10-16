using MathslideLearning.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MathslideLearning.Data.Interfaces
{
    public interface IExamRepository
    {
        Task<IEnumerable<Exam>> GetAllAsync();
        Task<Exam> CreateAsync(Exam exam);
        Task<Exam> GetByIdAsync(int id);
        Task<IEnumerable<Exam>> GetByTeacherIdAsync(int teacherId);
        Task<Exam> UpdateAsync(Exam exam);
        Task<bool> DeleteAsync(int id);
        Task<bool> AddQuestionsToExamAsync(int examId, List<int> questionIds);
        Task<bool> RemoveQuestionFromExamAsync(int examId, int questionId);

    }
}
