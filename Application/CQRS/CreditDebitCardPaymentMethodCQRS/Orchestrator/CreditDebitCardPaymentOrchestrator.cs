using APICoursePlatform.Helpers;
using Application.CQRS.CartItemCQRS.Commands;
using Application.CQRS.CartItemCQRS.Queries;
using Application.CQRS.CoursePaymentCQRS.Command;
using Application.CQRS.CreditDebitCardPaymentMethodCQRS.Command;
using Application.CQRS.PaymentCQRS.Command;
using Application.DTOs.CreditDebitCardPaymentMethodDTOs;
using Application.DTOs.PaymentCourseDTOs;
using Application.DTOs.PaymentDTO;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.CreditDebitCardPaymentMethodCQRS.Orchestrator
{
    // Command
    public class CreditDebitCardPaymentOrchestrator : IRequest<GeneralResponse<string>>
    {
        public string UserId { get; set; }
        public CreateCreditDebitCardPaymentMethodDto CardDto { get; set; }
        public CreatePaymentDto PaymentDto { get; set; }
    }

    // Handler
    public class CreditDebitCardPaymentOrchestratorHandler
        : IRequestHandler<CreditDebitCardPaymentOrchestrator, GeneralResponse<string>>
    {
        private readonly IMediator _mediator;

        public CreditDebitCardPaymentOrchestratorHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<GeneralResponse<string>> Handle(CreditDebitCardPaymentOrchestrator request, CancellationToken cancellationToken)
        {
            try
            {
                // 1. Get cart items
                var cartResponse = await _mediator.Send(new GetMyCartItemsQuery
                {
                    UserId = request.UserId,
                    PageNumber = 1,
                    PageSize = int.MaxValue
                }, cancellationToken);

                if (!cartResponse.Success || cartResponse.Data == null || !cartResponse.Data.Items.Any())
                    return GeneralResponse<string>.FailResponse("Your cart is empty.");

                var cartItems = cartResponse.Data.Items;

                // 2. Create Credit/Debit Card Payment Method
                var cardResponse = await _mediator.Send(
                    new CreateCreditDebitCardPaymentMethodCommand(request.CardDto),
                    cancellationToken);

                if (!cardResponse.Success || cardResponse.Data <= 0)
                    return GeneralResponse<string>.FailResponse(cardResponse.Message);

                int paymentMethodId = cardResponse.Data;

                // 3. Create Payment
                
                decimal Amount = cartItems.Sum(c => c.Price);

                var paymentResponse = await _mediator.Send(new CreatePaymentCommand(request.PaymentDto, request.UserId,paymentMethodId,Amount), cancellationToken);

                if (!paymentResponse.Success || paymentResponse.Data == null)
                    return GeneralResponse<string>.FailResponse(paymentResponse.Message);

                var payment = paymentResponse.Data;

                // 4. Link courses to payment
                foreach (var cartItem in cartItems)
                {
                    var dto = new CreatePaymentCourseDto
                    {
                        PaymentId = payment.Id,
                        CourseId = cartItem.CourseId
                    };

                    var linkResponse = await _mediator.Send(new CreatePaymentCourseCommand(dto), cancellationToken);

                    if (!linkResponse.Success)
                        return GeneralResponse<string>.FailResponse($"Failed to link course {cartItem.CourseTitle}: {linkResponse.Message}");
                }

                // 5. Delete cart items
                foreach (var item in cartItems)
                {
                    await _mediator.Send(new DeleteCartItemCommand
                    {
                        CartItemId = item.Id,
                        UserId = request.UserId
                    }, cancellationToken);
                }

                return GeneralResponse<string>.SuccessResponse("Credit/Debit Card payment completed successfully.", payment.Id.ToString());
            }
            catch (Exception ex)
            {
                return GeneralResponse<string>.FailResponse($"Unexpected error: {ex.Message}");
            }
        }
    }
}
