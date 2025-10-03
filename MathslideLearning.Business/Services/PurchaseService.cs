using MathslideLearning.Business.Interfaces;
using MathslideLearning.Data.Entities;
using MathslideLearning.Data.Interfaces;
using MathslideLearning.Models.PaymentDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MathslideLearning.Business.Services
{
    public class PurchaseService : IPurchaseService
    {
        private readonly IReceiptRepository _receiptRepository;
        private readonly ISlideRepository _slideRepository;
        private readonly IPaymentMethodRepository _paymentMethodRepository;

        public PurchaseService(IReceiptRepository receiptRepository, ISlideRepository slideRepository, IPaymentMethodRepository paymentMethodRepository)
        {
            _receiptRepository = receiptRepository;
            _slideRepository = slideRepository;
            _paymentMethodRepository = paymentMethodRepository;
        }

        public async Task<ReceiptResponseDto> PurchaseSlidesAsync(int studentId, PurchaseRequestDto purchaseRequest)
        {
            var slidesToPurchase = new List<Slide>();
            decimal calculatedTotalPrice = 0;

            var paymentMethod = await _paymentMethodRepository.GetByIdAsync(purchaseRequest.PaymentMethodId);
            if (paymentMethod == null)
            {
                throw new Exception("Invalid Payment Method ID.");
            }

            foreach (var slideId in purchaseRequest.SlideIds.Distinct())
            {
                if (await _receiptRepository.HasUserPurchasedSlideAsync(studentId, slideId))
                {
                    throw new Exception($"You have already purchased slide with ID {slideId}.");
                }

                var slide = await _slideRepository.GetSlideByIdAsync(slideId);
                if (slide == null || !slide.IsPublished)
                {
                    throw new Exception($"Slide with ID {slideId} is not available for purchase.");
                }

                slidesToPurchase.Add(slide);
                calculatedTotalPrice += slide.Price;
            }

            var newReceipt = new Receipt
            {
                UserId = studentId,
                PaymentMethodId = purchaseRequest.PaymentMethodId,
                Status = "Pending",
                CreatedAt = DateTime.UtcNow,
                ReceiptDetails = slidesToPurchase.Select(s => new ReceiptDetail { SlideId = s.Id }).ToList()
            };

            var createdReceipt = await _receiptRepository.CreateReceiptAsync(newReceipt);
            var fullReceipt = await _receiptRepository.GetReceiptByIdAsync(createdReceipt.Id);

            return new ReceiptResponseDto
            {
                Id = fullReceipt.Id,
                UserId = fullReceipt.UserId,
                PaymentMethod = fullReceipt.PaymentMethod?.Name ?? "N/A",
                TotalPrice = calculatedTotalPrice,
                Status = fullReceipt.Status,
                CreatedAt = fullReceipt.CreatedAt,
                PurchasedItems = fullReceipt.ReceiptDetails.Select(rd => new PurchasedItemDto
                {
                    SlideId = rd.SlideId,
                    Title = rd.Slide.Title,
                    FileUrl = rd.Slide.FileUrl
                }).ToList()
            };
        }
        public async Task<ReceiptResponseDto> UpdateReceiptStatusAsync(int receiptId, string status)
        {
            var receiptToUpdate = await _receiptRepository.GetReceiptByIdAsync(receiptId);
            if (receiptToUpdate == null)
            {
                throw new Exception("Receipt not found.");
            }

            receiptToUpdate.Status = status;
            await _receiptRepository.UpdateReceiptAsync(receiptToUpdate);

            var fullReceipt = await _receiptRepository.GetReceiptByIdAsync(receiptId);
            var totalPrice = fullReceipt.ReceiptDetails.Sum(rd => rd.Slide.Price);

            return new ReceiptResponseDto
            {
                Id = fullReceipt.Id,
                UserId = fullReceipt.UserId,
                PaymentMethod = fullReceipt.PaymentMethod?.Name ?? "N/A",
                TotalPrice = totalPrice,
                Status = fullReceipt.Status,
                CreatedAt = fullReceipt.CreatedAt,
                PurchasedItems = fullReceipt.ReceiptDetails.Select(rd => new PurchasedItemDto
                {
                    SlideId = rd.SlideId,
                    Title = rd.Slide.Title,
                    FileUrl = rd.Slide.FileUrl
                }).ToList()
            };
        }

        public async Task<IEnumerable<ReceiptResponseDto>> GetPurchaseHistoryAsync(int studentId)
        {
            var receipts = await _receiptRepository.GetReceiptsByUserIdAsync(studentId);

            return receipts
                .Where(receipt => receipt.Status == "Paid") 
                .Select(receipt =>
                {
                    var totalPrice = receipt.ReceiptDetails.Sum(rd => rd.Slide.Price);
                    return new ReceiptResponseDto
                    {
                        Id = receipt.Id,
                        UserId = receipt.UserId,
                        PaymentMethod = receipt.PaymentMethod?.Name ?? "N/A",
                        TotalPrice = totalPrice,
                        Status = receipt.Status,
                        CreatedAt = receipt.CreatedAt,
                        PurchasedItems = receipt.ReceiptDetails.Select(rd => new PurchasedItemDto
                        {
                            SlideId = rd.SlideId,
                            Title = rd.Slide.Title,
                            FileUrl = rd.Slide.FileUrl
                        }).ToList()
                    };
                });
        }
    }
}