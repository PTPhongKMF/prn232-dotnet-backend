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
            return Api200(paymentMethods);
        }

        [HttpGet("{id}")]
        [AllowAnonymous] // Allow everyone to see a single payment method
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var paymentMethod = await _paymentMethodService.GetPaymentMethodByIdAsync(id);
                return Api200(paymentMethod);
            }
            catch (Exception ex)
            {
                return Api404<object>(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PaymentMethodRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return Api400<ModelStateDictionary>("Validation failed", ModelState);
            }

            try
            {
                var newPaymentMethod = await _paymentMethodService.CreatePaymentMethodAsync(request);
                return Api201(newPaymentMethod);
            }
            catch (Exception ex)
            {
                return Api400<object>(ex.Message, new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] PaymentMethodRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return Api400<ModelStateDictionary>("Validation failed", ModelState);
            }

            try
            {
                var updatedMethod = await _paymentMethodService.UpdatePaymentMethodAsync(id, request);
                return Api200(updatedMethod);
            }
            catch (Exception ex)
            {
                return Api400<object>(ex.Message, new { message = ex.Message });
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
                    return Api404<object>("Payment method not found");
                }
                return Api200<object>("Payment method deleted successfully", null);
            }
            catch (Exception ex)
            {
                return Api400<object>(ex.Message, new { message = ex.Message });
            }
        }
    }
}