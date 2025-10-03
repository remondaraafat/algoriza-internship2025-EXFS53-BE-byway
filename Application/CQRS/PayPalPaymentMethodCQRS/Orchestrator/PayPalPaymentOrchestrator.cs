using APICoursePlatform.Helpers;
using Application.CQRS.CartItemCQRS.Commands;
using Application.CQRS.CartItemCQRS.Queries;
using Application.CQRS.CoursePaymentCQRS.Command;
using Application.CQRS.PaymentCQRS.Command;
using Application.CQRS.PayPalPaymentMethodCQRS.Command;
using Application.DTOs.PaymentCourseDTOs;
using Application.DTOs.PaymentDTO;
using Application.DTOs.PayPalPaymentMethodDTOs;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Application.Orchestrators
{
    // Command
    public class PayPalPaymentOrchestrator : IRequest<GeneralResponse<string>>
    {
        public string UserId { get; set; }
        public string PayPalEmail { get; set; }
        public CreatePaymentDto PaymentDto { get; set; }
    }
    // Handler
    public class PayPalPaymentOrchestratorHandler : IRequestHandler<PayPalPaymentOrchestrator, GeneralResponse<string>>
    {
        private readonly IMediator _mediator;
        private readonly IEmailService _emailService;
        private readonly IHttpContextAccessor _httpContextAccessor = new HttpContextAccessor();
        public PayPalPaymentOrchestratorHandler(IMediator mediator, IEmailService emailService, IHttpContextAccessor httpContextAccessor)
        {
            _mediator = mediator;
            _emailService = emailService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<GeneralResponse<string>> Handle(PayPalPaymentOrchestrator request, CancellationToken cancellationToken)
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

                // 2. Create PayPal Payment Method
                var paypalResponse = await _mediator.Send(new CreatePayPalPaymentMethodCommand
                {
                    Dto = new CreatePayPalPaymentMethodDto { PayPalEmail = request.PayPalEmail }
                }, cancellationToken);

                if (!paypalResponse.Success || string.IsNullOrEmpty(paypalResponse.Data))
                    return GeneralResponse<string>.FailResponse(paypalResponse.Message);

                int paymentMethodId = int.Parse(paypalResponse.Data);

                // 3. Create Payment
               
                decimal Amount = cartItems.Sum(c => c.Price);

                var paymentResponse = await _mediator.Send(new CreatePaymentCommand(request.PaymentDto, request.UserId,paymentMethodId,Amount), cancellationToken);

                if (!paymentResponse.Success || paymentResponse.Data == null)
                    return GeneralResponse<string>.FailResponse(paymentResponse.Message);

                var payment = paymentResponse.Data;

                // 4. Loop through courses and link them
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
                // 5. delete cart items  
                foreach (var item in cartItems)
                {
                    await _mediator.Send(new DeleteCartItemCommand
                    {
                        CartItemId = item.Id,
                        UserId = request.UserId
                    }, cancellationToken);
                }
                //user email 
                var userEmail = _httpContextAccessor.HttpContext.User.FindFirst("email")?.Value;

                var firstName = _httpContextAccessor.HttpContext.User.FindFirst("given_name")?.Value
             ?? _httpContextAccessor.HttpContext.User.FindFirst("firstName")?.Value;



                await _emailService.SendEmailAsync(
    toEmail: userEmail,
    subject: "Thank you for your purchase! 🎉",
    body: $@"
        <div style='max-width:600px;margin:auto;font-family:Arial;padding:30px;
                    background:#f9f9f9;border-radius:10px;border:1px solid #ddd;color:#333'>
            <h1 style='color:#2a7ae2;text-align:center'>Thank you, {firstName}! 🎉</h1>
            <p style='font-size:16px;text-align:center'>
                Your courses are now available in your dashboard.  
                Best of luck on your learning journey 🚀
            </p>
            <hr style='margin:30px 0;border:none;border-top:1px solid #eee'>
            <footer style='font-size:13px;color:#888;text-align:center'>
                © {DateTime.Now.Year} Course Platform. All rights reserved.
            </footer>
        </div>"
);

                return GeneralResponse<string>.SuccessResponse("PayPal payment completed successfully.", payment.Id.ToString());
            }
            catch (Exception ex)
            {
                return GeneralResponse<string>.FailResponse($"Unexpected error: {ex.Message}");
            }
        }
    }
}
