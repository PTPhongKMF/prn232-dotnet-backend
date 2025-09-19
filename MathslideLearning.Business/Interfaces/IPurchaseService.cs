using MathslideLearning.Common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MathslideLearning.Business.Interfaces
{
    public interface IPurchaseService
    {
        Task<ReceiptResponseDto> PurchaseSlidesAsync(int studentId, PurchaseRequestDto purchaseRequest);
        Task<IEnumerable<ReceiptResponseDto>> GetPurchaseHistoryAsync(int studentId);
    }
}