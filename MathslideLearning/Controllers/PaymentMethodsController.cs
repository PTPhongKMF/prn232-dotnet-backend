using MathslideLearning.Business.Interfaces;
using MathslideLearning.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace MathslideLearning.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")] // Secure this entire controller for Admins only
    public class PaymentMethodsController : ControllerBase
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
            return Ok(paymentMethods);
        }

        [HttpGet("{id}")]
        [AllowAnonymous] // Allow everyone to see a single payment method
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var paymentMethod = await _paymentMethodService.GetPaymentMethodByIdAsync(id);
                return Ok(paymentMethod);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PaymentMethodRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var newPaymentMethod = await _paymentMethodService.CreatePaymentMethodAsync(request);
                return CreatedAtAction(nameof(GetById), new { id = newPaymentMethod.Id }, newPaymentMethod);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] PaymentMethodRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var updatedMethod = await _paymentMethodService.UpdatePaymentMethodAsync(id, request);
                return Ok(updatedMethod);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
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
                    return NotFound(new { message = "Payment method not found." });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}

