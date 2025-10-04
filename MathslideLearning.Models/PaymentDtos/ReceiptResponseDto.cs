using System;
using System.Collections.Generic;

namespace MathslideLearning.Models.PaymentDtos
{
    public class ReceiptResponseDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string PaymentMethod { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<string> PurchasedSlideTitles { get; set; }
        public List<PurchasedItemDto> PurchasedItems { get; set; }
    }
}