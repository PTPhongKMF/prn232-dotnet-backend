using MathslideLearning.Business.Interfaces;
using MathslideLearning.Controllers.Base;
using MathslideLearning.Models.PaymentDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Threading.Tasks;

namespace MathslideLearning.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")] // Secure this entire controller for Admins only
    public class PaymentMethodsController : ApiControllerBase
    {
        private readonly IPaymentMethodService _paymentMethodService;

        public PaymentMethodsController(IPaymentMethodService paymentMethodService)
        {
            _paymentMethodService = paymentMethodService;
        }

        [HttpGet]
        [AllowAnonymous] // Allow everyone to see the list of payment methods
        public async Task<IActionResult> GetAll()
        {
            var paymentMethods = await _paymentMethodService.GetAllPaymentMethodsAsync();
            return ApiOk(paymentMethods);
        }

        [HttpGet("{id}")]
        [AllowAnonymous] // Allow everyone to see a single payment method
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var paymentMethod = await _paymentMethodService.GetPaymentMethodByIdAsync(id);
                return ApiOk(paymentMethod);
            }
            catch (Exception ex)
            {
                return ApiNotFound<object>(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PaymentMethodRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return ApiBadRequest<ModelStateDictionary>(ModelState, "Validation failed");
            }

            try
            {
                var newPaymentMethod = await _paymentMethodService.CreatePaymentMethodAsync(request);
                return ApiCreated(newPaymentMethod);
            }
            catch (Exception ex)
            {
                return ApiBadRequest<object>(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] PaymentMethodRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return ApiBadRequest<ModelStateDictionary>(ModelState, "Validation failed");
            }

            try
            {
                var updatedMethod = await _paymentMethodService.UpdatePaymentMethodAsync(id, request);
                return ApiOk(updatedMethod);
            }
            catch (Exception ex)
            {
                return ApiBadRequest<object>(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var success = await _paymentMethodService.DeletePaymentMethodAsync(id);
                if (!success)
                {
                    return ApiNotFound<object>("Payment method not found");
                }
                return ApiOk<object>(null, "Payment method deleted successfully");
            }
            catch (Exception ex)
            {
                return ApiBadRequest<object>(new { message = ex.Message });
            }
        }
    }
}