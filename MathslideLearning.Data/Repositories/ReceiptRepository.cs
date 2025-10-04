using MathslideLearning.Data.DbContext;
using MathslideLearning.Data.Entities;
using MathslideLearning.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MathslideLearning.Data.Repositories
{
    public class ReceiptRepository : IReceiptRepository
    {
        private readonly MathslideLearningDbContext _context;

        public ReceiptRepository(MathslideLearningDbContext context)
        {
            _context = context;
        }

        public async Task<Receipt> UpdateReceiptAsync(Receipt receipt)
        {
            _context.Receipts.Update(receipt);
            await _context.SaveChangesAsync();
            return receipt;
        }

        public async Task<IEnumerable<ReceiptDetail>> GetSalesByTeacherIdAsync(int teacherId)
        {
            return await _context.ReceiptDetails
                .Include(rd => rd.Receipt)
                    .ThenInclude(r => r.User)
                .Include(rd => rd.Slide)
                .Where(rd => rd.Slide.TeacherId == teacherId && rd.Receipt.Status == "Completed")
                .OrderByDescending(rd => rd.Receipt.CreatedAt)
                .ToListAsync();
        }
        public async Task<Receipt> CreateReceiptAsync(Receipt receipt)
        {
            await _context.Receipts.AddAsync(receipt);
            await _context.SaveChangesAsync();
            return receipt;
        }

        public async Task<bool> HasUserPurchasedSlideAsync(int userId, int slideId)
        {
            return await _context.Receipts
                .AnyAsync(r => r.UserId == userId && r.Status == "Completed" &&
                               r.ReceiptDetails.Any(rd => rd.SlideId == slideId));
        }

        public async Task<Receipt> GetReceiptByIdAsync(int receiptId)
        {
            return await _context.Receipts
                .Include(r => r.PaymentMethod)
                .Include(r => r.ReceiptDetails)
                    .ThenInclude(rd => rd.Slide)
                .FirstOrDefaultAsync(r => r.Id == receiptId);
        }

        public async Task<IEnumerable<Receipt>> GetReceiptsByUserIdAsync(int userId)
        {
            return await _context.Receipts
                .Include(r => r.PaymentMethod)
                .Include(r => r.ReceiptDetails)
                    .ThenInclude(rd => rd.Slide)
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }
    }
}
