using MathslideLearning.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MathslideLearning.Data.Interfaces
{
    public interface IReceiptRepository
    {
        Task<Receipt> CreateReceiptAsync(Receipt receipt);
        Task<bool> HasUserPurchasedSlideAsync(int userId, int slideId);
        Task<Receipt> GetReceiptByIdAsync(int receiptId);
        Task<IEnumerable<Receipt>> GetReceiptsByUserIdAsync(int userId);
        Task<IEnumerable<ReceiptDetail>> GetSalesByTeacherIdAsync(int teacherId);
        Task<Receipt> UpdateReceiptAsync(Receipt receipt);
    }
}