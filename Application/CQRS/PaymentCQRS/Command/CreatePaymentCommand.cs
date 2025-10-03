using APICoursePlatform.Helpers;
using APICoursePlatform.UnitOfWorkContract;
using Application.DTOs.PaymentDTO;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.PaymentCQRS.Command
{
    // 1. Command
    public class CreatePaymentCommand : IRequest<GeneralResponse<Payment>>
    {
        [Required(ErrorMessage = "User Id is required.")]
        public string UserId { get; set; }
        public CreatePaymentDto PaymentDto { get; set; }
        [Required(ErrorMessage = "Amount is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }



        [Required(ErrorMessage = "Payment Method Id is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Payment Method Id must be greater than 0.")]
        public int PaymentMethodId { get; set; }
        public CreatePaymentCommand(CreatePaymentDto dto,string userId, int paymentMethodId, decimal amount)
        {
            PaymentDto = dto;
            UserId = userId;
            Amount= amount;
            PaymentMethodId = paymentMethodId;
        }
    }

    // 2. Handler
    public class CreatePaymentCommandHandler : IRequestHandler<CreatePaymentCommand, GeneralResponse<Payment>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreatePaymentCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GeneralResponse<Payment>> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dto = request.PaymentDto;

                // Map DTO -> Entity
                var payment = new Payment
                {
                    Country = dto.Country,
                    State = dto.State,
                    Amount = request.Amount,
                    UserId = request.UserId,
                    PaymentMethodId = request.PaymentMethodId,
                    
                };

                // Save to DB
                await _unitOfWork.paymentRepository.AddAsync(payment);
                await _unitOfWork.SaveAsync();

                return GeneralResponse<Payment>.SuccessResponse(
                    "Payment created successfully.",
                    payment
                );
            }
            catch (Exception ex)
            {
                return GeneralResponse<Payment>.FailResponse(
                    $"Error while creating Payment: {ex.Message}"
                );
            }
        }
    }
}
