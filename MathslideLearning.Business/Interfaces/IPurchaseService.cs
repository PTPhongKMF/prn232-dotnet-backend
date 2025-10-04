using MathslideLearning.Models.PaymentDtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MathslideLearning.Business.Interfaces
{
    public interface IPurchaseService
    {
        Task<ReceiptResponseDto> PurchaseSlidesAsync(int studentId, PurchaseRequestDto purchaseRequest);
        Task<IEnumerable<ReceiptResponseDto>> GetPurchaseHistoryAsync(int studentId);
        Task<ReceiptResponseDto> UpdateReceiptStatusAsync(int receiptId, string status);
    }
}