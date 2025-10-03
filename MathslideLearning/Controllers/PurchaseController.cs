using MathslideLearning.Business.Interfaces;
using MathslideLearning.Controllers.Base;
using MathslideLearning.Models.PaymentDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MathslideLearning.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Student")]
    public class PurchaseController : ApiControllerBase
    {
        private readonly IPurchaseService _purchaseService;

        public PurchaseController(IPurchaseService purchaseService)
        {
            _purchaseService = purchaseService;
        }

        [HttpPost]
        public async Task<IActionResult> PurchaseSlides([FromBody] PurchaseRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return Api400<ModelStateDictionary>("Validation failed", ModelState);
            }

            try
            {
                var studentId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var receipt = await _purchaseService.PurchaseSlidesAsync(studentId, request);
                return Api200(receipt);
            }
            catch (Exception ex)
            {
                return Api400<object>(ex.Message, new { message = ex.Message });
            }
        }
        [HttpPatch("{receiptId}/status")]
        public async Task<IActionResult> UpdateReceiptStatus(int receiptId, [FromBody] ReceiptStatusUpdateDto request)
        {
            if (!ModelState.IsValid)
            {
                return Api400<ModelStateDictionary>("Validation failed", ModelState);
            }

            try
            {
                var studentId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var receipt = await _purchaseService.UpdateReceiptStatusAsync(receiptId, request.Status);

                return Api200(receipt);
            }
            catch (Exception ex)
            {
                return Api400<object>(ex.Message, new { message = ex.Message });
            }
        }
        [HttpGet("history")]
        public async Task<IActionResult> GetMyPurchaseHistory()
        {
            try
            {
                var studentId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var history = await _purchaseService.GetPurchaseHistoryAsync(studentId);
                return Api200(history);
            }
            catch (Exception ex)
            {
                return Api404<object>(ex.Message);
            }
        }
    }
}