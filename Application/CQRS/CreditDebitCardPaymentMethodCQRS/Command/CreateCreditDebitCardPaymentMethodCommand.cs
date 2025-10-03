using APICoursePlatform.Helpers;
using APICoursePlatform.UnitOfWorkContract;
using Application.DTOs.CreditDebitCardPaymentMethodDTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static APICoursePlatform.Enums.Enums;

namespace Application.CQRS.CreditDebitCardPaymentMethodCQRS.Command
{
    // Command
    public class CreateCreditDebitCardPaymentMethodCommand
        : IRequest<GeneralResponse<int>>
    {
        public CreateCreditDebitCardPaymentMethodDto Dto { get; set; }

        public CreateCreditDebitCardPaymentMethodCommand(CreateCreditDebitCardPaymentMethodDto dto)
        {
            Dto = dto;
        }
    }

    // Handler
    public class CreateCreditDebitCardPaymentMethodCommandHandler
        : IRequestHandler<CreateCreditDebitCardPaymentMethodCommand, GeneralResponse<int>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateCreditDebitCardPaymentMethodCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GeneralResponse<int>> Handle(
            CreateCreditDebitCardPaymentMethodCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                var dto = request.Dto;
                
                // Map DTO → Entity
                var entity = new CreditDebitCardPaymentMethod
                {
                    MethodType = PaymentMethodType.CreditDebitCard,
                    CardName = dto.CardName,
                    CardNumber = dto.CardNumber,
                    Expiry = dto.Expiry,
                    CVV = dto.CVV
                };

                await _unitOfWork.creditDebitCardPaymentMethodRepository.AddAsync(entity);
                await _unitOfWork.SaveAsync();

                return GeneralResponse<int>.SuccessResponse(
                    "Credit/Debit card payment method created successfully.",
                    entity.Id
                );
            }
            catch (System.Exception ex)
            {
                return GeneralResponse<int>.FailResponse($"Error: {ex.Message}");
            }
        }
    }
}
