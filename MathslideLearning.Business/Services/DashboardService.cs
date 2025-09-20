using MathslideLearning.Business.Interfaces;
using MathslideLearning.Data.Interfaces;
using MathslideLearning.Models.PaymentDtos;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MathslideLearning.Business.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IReceiptRepository _receiptRepository;

        public DashboardService(IReceiptRepository receiptRepository)
        {
            _receiptRepository = receiptRepository;
        }

        public async Task<IEnumerable<TeacherSaleDto>> GetSalesHistoryForTeacherAsync(int teacherId)
        {
            var sales = await _receiptRepository.GetSalesByTeacherIdAsync(teacherId);

            return sales.Select(sale => new TeacherSaleDto
            {
                ReceiptId = sale.ReceiptId,
                SlideTitle = sale.Slide.Title,
                SalePrice = sale.Slide.Price,
                StudentName = sale.Receipt.User.Name,
                PurchaseDate = sale.Receipt.CreatedAt
            });
        }
    }
}
