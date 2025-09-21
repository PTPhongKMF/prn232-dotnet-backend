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
    public class PaymentMethodService : IPaymentMethodService
    {
        private readonly IPaymentMethodRepository _paymentMethodRepository;

        public PaymentMethodService(IPaymentMethodRepository paymentMethodRepository)
        {
            _paymentMethodRepository = paymentMethodRepository;
        }

        public async Task<IEnumerable<PaymentMethodDto>> GetAllPaymentMethodsAsync()
        {
            var paymentMethods = await _paymentMethodRepository.GetAllAsync();
            return paymentMethods.Select(pm => new PaymentMethodDto { Id = pm.Id, Name = pm.Name });
        }

        public async Task<PaymentMethodDto> GetPaymentMethodByIdAsync(int id)
        {
            var paymentMethod = await _paymentMethodRepository.GetByIdAsync(id);
            if (paymentMethod == null)
            {
                throw new Exception("Payment method not found.");
            }
            return new PaymentMethodDto { Id = paymentMethod.Id, Name = paymentMethod.Name };
        }

        public async Task<PaymentMethodDto> CreatePaymentMethodAsync(PaymentMethodRequestDto request)
        {
            if (await _paymentMethodRepository.ExistsAsync(request.Name))
            {
                throw new Exception($"A payment method with the name '{request.Name}' already exists.");
            }

            var newPaymentMethod = new PaymentMethod { Name = request.Name };
            var createdMethod = await _paymentMethodRepository.CreateAsync(newPaymentMethod);

            return new PaymentMethodDto { Id = createdMethod.Id, Name = createdMethod.Name };
        }

        public async Task<PaymentMethodDto> UpdatePaymentMethodAsync(int id, PaymentMethodRequestDto request)
        {
            var paymentMethodToUpdate = await _paymentMethodRepository.GetByIdAsync(id);
            if (paymentMethodToUpdate == null)
            {
                throw new Exception("Payment method not found.");
            }

            if (await _paymentMethodRepository.ExistsAsync(request.Name))
            {
                throw new Exception($"A payment method with the name '{request.Name}' already exists.");
            }

            paymentMethodToUpdate.Name = request.Name;
            var updatedMethod = await _paymentMethodRepository.UpdateAsync(paymentMethodToUpdate);

            return new PaymentMethodDto { Id = updatedMethod.Id, Name = updatedMethod.Name };
        }

        public async Task<bool> DeletePaymentMethodAsync(int id)
        {
            var paymentMethod = await _paymentMethodRepository.GetByIdAsync(id);
            if (paymentMethod == null)
            {
                throw new Exception("Payment method not found.");
            }
            return await _paymentMethodRepository.DeleteAsync(id);
        }
    }
}

