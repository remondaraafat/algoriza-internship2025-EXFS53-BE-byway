using Application.CQRS.CartItemCQRS.Commands;
using Application.CQRS.CartItemCQRS.Queries;
using Application.DTOs.CartItemDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace API.Controllers
{
    [Authorize]
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
        public async Task<ActionResult<GeneralResponse<GetCartItemDto>>> Create([FromBody] int CourseId)
        {
            
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
              ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);
            CreateCartItemDto dto = new CreateCartItemDto { CourseId = CourseId, UserId = userId };
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

        // Get My Cart Items
        [HttpGet("my-cart")]
        public async Task<ActionResult<GeneralResponse<PagedResult<GetCartItemDto>>>> GetMyCartItems(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);

            var query = new GetMyCartItemsQuery
            {
                UserId = userId,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var result = await _mediator.Send(query);

            return Ok(result); 
        }

    }
}
