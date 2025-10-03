
using Application.CQRS.CreditDebitCardPaymentMethodCQRS.Command;
using Application.CQRS.CreditDebitCardPaymentMethodCQRS.Orchestrator;
using Application.CQRS.PaymentCQRS.Query;
using Application.DTOs.CreditDebitCardPaymentMethodDTOs;
using Application.DTOs.PaymentDTO;
using Application.Orchestrators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

[ApiController]

[Route("api/[controller]")]

public class PaymentController : Controller
{
    private readonly IMediator _mediator;

    public PaymentController(IMediator mediator)
    {
        _mediator = mediator;
    }
    // Get total payments for the current month
    [Authorize(Roles = "Admin")]
    [HttpGet("TotalForCurrentMonth")]
    public async Task<ActionResult<GeneralResponse<decimal>>> GetTotalForCurrentMonth()
    {
        var query = new GetTotalPaymentsForCurrentMonthQuery();
        var result = await _mediator.Send(query);

        return Ok(result);
    }
    // Pay with PayPal
    [Authorize(Roles = "User")]
    [HttpPost("paypal")]
    public async Task<ActionResult<GeneralResponse<string>>> PayWithPayPal(
    [FromBody] CreatePaymentDto dto,
    [FromQuery] string paypalEmail,
    CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
              ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized(GeneralResponse<string>.FailResponse("User not authenticated."));

        var command = new PayPalPaymentOrchestrator
        {
            UserId = userId,
            PayPalEmail = paypalEmail,
            PaymentDto = dto
        };

        var result = await _mediator.Send(command, cancellationToken);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }
    // Pay with Credit/Debit Card
    [Authorize(Roles = "User")]
    [HttpPost("credit-debit-card")]
    public async Task<ActionResult<GeneralResponse<string>>> PayWithCreditDebitCard(
                [FromBody] CreateCreditDebitCardPaymentMethodDto cardDto,
                [FromQuery] CreatePaymentDto paymentDto,
                CancellationToken cancellationToken)
    {
        // Get the authenticated user ID from token claims
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
            return Unauthorized(GeneralResponse<string>.FailResponse("User not authenticated."));

        var command = new CreditDebitCardPaymentOrchestrator
        {
            UserId = userId,
            CardDto = cardDto,
            PaymentDto = paymentDto
        };

        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

}

