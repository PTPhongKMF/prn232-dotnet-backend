using MathslideLearning.Models.PaymentDtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MathslideLearning.Business.Interfaces
{
    public interface IPaymentMethodService
    {
        Task<IEnumerable<PaymentMethodDto>> GetAllPaymentMethodsAsync();
        Task<PaymentMethodDto> GetPaymentMethodByIdAsync(int id);
        Task<PaymentMethodDto> CreatePaymentMethodAsync(PaymentMethodRequestDto request);
        Task<PaymentMethodDto> UpdatePaymentMethodAsync(int id, PaymentMethodRequestDto request);
        Task<bool> DeletePaymentMethodAsync(int id);
    }
}