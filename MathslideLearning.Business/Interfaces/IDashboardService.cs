using MathslideLearning.Models.PaymentDtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MathslideLearning.Business.Interfaces
{
    public interface IDashboardService
    {
        Task<IEnumerable<TeacherSaleDto>> GetSalesHistoryForTeacherAsync(int teacherId);
    }
}