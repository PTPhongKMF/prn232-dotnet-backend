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
                return ApiBadRequest<ModelStateDictionary>(ModelState, "Validation failed");
            }

            try
            {
                var studentId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var receipt = await _purchaseService.PurchaseSlidesAsync(studentId, request);
                return ApiOk(receipt);
            }
            catch (Exception ex)
            {
                return ApiBadRequest<object>(new { message = ex.Message });
            }
        }

        [HttpGet("history")]
        public async Task<IActionResult> GetMyPurchaseHistory()
        {
            try
            {
                var studentId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var history = await _purchaseService.GetPurchaseHistoryAsync(studentId);
                return ApiOk(history);
            }
            catch (Exception ex)
            {
                return ApiNotFound<object>(ex.Message);
            }
        }
    }
}