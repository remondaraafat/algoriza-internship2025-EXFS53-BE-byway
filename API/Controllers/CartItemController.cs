using Application.CQRS.CartItemCQRS.Commands;
using Application.CQRS.CartItemCQRS.Queries;
using Application.DTOs.CartItemDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartItemController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CartItemController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // Create CartItem
        [HttpPost]
        public async Task<ActionResult<GeneralResponse<GetCartItemDto>>> Create([FromBody] CreateCartItemDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
              ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);
            dto.UserId = userId;
            return Ok(await _mediator.Send(new CreateCartItemCommand { Dto = dto }));
        }

        // Delete CartItem
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<GeneralResponse<bool>>> Delete(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
              ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);


            var result = await _mediator.Send(new DeleteCartItemCommand { CartItemId = id , UserId = userId });

            if (!result.Success)
                return NotFound(GeneralResponse<bool>.FailResponse("Cart item not found"));

            return Ok(GeneralResponse<bool>.SuccessResponse("Cart item deleted successfully", true));
        }

        //  Get My Cart Items
        [HttpGet("my-cart/{userId}")]
        public async Task<ActionResult<GeneralResponse<List<GetCartItemDto>>>> GetMyCartItems()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
              ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);

            var query = new GetMyCartItemsQuery { UserId = userId };
            var result = await _mediator.Send(query);

            return Ok(GeneralResponse<List<GetCartItemDto>>.SuccessResponse("Cart items retrieved successfully", result));
        }
    }
}
