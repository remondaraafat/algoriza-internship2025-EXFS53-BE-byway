using Application.CQRS.CategoryCQRS.Query;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CategoriesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        
        [HttpGet("names")]
        public async Task<IActionResult> GetCategoryNames([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var query = new GetCategoryNamesQuery
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var result = await _mediator.Send(query);

            return Ok(result);
        }
        [HttpGet("Count")]
        public async Task<ActionResult<GeneralResponse<int>>> GetCategoriesCount()
        {
            var query = new GetCategoriesCountQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}

