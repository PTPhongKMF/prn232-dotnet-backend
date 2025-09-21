using MathslideLearning.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MathslideLearning.Data.Interfaces
{
    public interface IExamRepository
    {
        Task<Exam> CreateAsync(Exam exam);
        Task<Exam> GetByIdAsync(int id);
        Task<IEnumerable<Exam>> GetByTeacherIdAsync(int teacherId);
        Task<Exam> UpdateAsync(Exam exam);
        Task<bool> DeleteAsync(int id);
    }
}
