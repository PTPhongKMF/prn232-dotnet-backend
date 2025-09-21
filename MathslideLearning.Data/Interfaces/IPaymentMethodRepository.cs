using MathslideLearning.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MathslideLearning.Data.Interfaces
{
    public interface IPaymentMethodRepository
    {
        Task<IEnumerable<PaymentMethod>> GetAllAsync();
        Task<PaymentMethod> GetByIdAsync(int id);
        Task<PaymentMethod> CreateAsync(PaymentMethod paymentMethod);
        Task<PaymentMethod> UpdateAsync(PaymentMethod paymentMethod);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(string name);
    }
}