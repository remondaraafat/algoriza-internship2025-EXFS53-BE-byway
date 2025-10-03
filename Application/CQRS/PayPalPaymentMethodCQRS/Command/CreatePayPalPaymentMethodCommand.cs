using APICoursePlatform.Enums;
using APICoursePlatform.Helpers;
using APICoursePlatform.UnitOfWorkContract;
using Application.DTOs.PayPalPaymentMethodDTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static APICoursePlatform.Enums.Enums;

namespace Application.CQRS.PayPalPaymentMethodCQRS.Command
{
    public class CreatePayPalPaymentMethodCommand : IRequest<GeneralResponse<string>>
    {
        public CreatePayPalPaymentMethodDto Dto { get; set; }
    }
    public class CreatePayPalPaymentMethodHandler : IRequestHandler<CreatePayPalPaymentMethodCommand, GeneralResponse<string>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreatePayPalPaymentMethodHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GeneralResponse<string>> Handle(CreatePayPalPaymentMethodCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Dto.PayPalEmail))
                    return GeneralResponse<string>.FailResponse("PayPal email is required.");

                var paymentMethod = new PayPalPaymentMethod
                {
                    MethodType = PaymentMethodType.PayPal,
                    PayPalEmail = request.Dto.PayPalEmail,
                };

                await _unitOfWork.payPalMethodRepository.AddAsync(paymentMethod);
                await _unitOfWork.SaveAsync();

                return GeneralResponse<string>.SuccessResponse("PayPal payment method created successfully.", paymentMethod.Id.ToString());
            }
            catch (Exception ex)
            {
                return GeneralResponse<string>.FailResponse($"An error occurred: {ex.Message}");
            }
        }
    }
}
