using Application.CQRS.CourseCQRS.Command;
using Application.CQRS.CourseCQRS.Command;
using Application.CQRS.CourseCQRS.Query;
using Application.DTOs.CourseDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CoursesController(IMediator mediator)
        {
            _mediator = mediator;
        }
        //create course
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateCourse([FromForm] CreateCourseDto dto)
        {
            var result = await _mediator.Send(new CreateCourseCommand{CourseDto=dto});
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
        //update course
        [Authorize(Roles = "Admin")]
        [HttpPut("update")]
        public async Task<IActionResult> UpdateCourse([FromForm] UpdateCourseDto dto)
        {
            var response = await _mediator.Send(new UpdateCourseCommand { Dto=dto});
            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }
        //delete course by id
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<GeneralResponse<string>>> DeleteCourse(int id)
        {
            var result = await _mediator.Send(new DeleteCourseCommand { Id=id});

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }
        //get all courses
        [HttpGet("all")]
        public async Task<ActionResult<GeneralResponse<PagedResult<GetAllCoursesDto>>>>
        GetAllCourses([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _mediator.Send(new GetAllCoursesQuery
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            });

            return Ok(result);
        }

        // Get course by id
        [HttpGet("{id}")]
        public async Task<ActionResult<GeneralResponse<GetCourseByIdDto>>> GetCourseById(int id)
        {
            string? userId = null;

            // Check if user is authenticated before trying to read claims
            if (User.Identity?.IsAuthenticated == true)
            {
                userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                         ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);
            }

            var result = await _mediator.Send(new GetCourseByIdQuery
            {
                Id = id,
                UserId = userId // could be null if user not logged in
            });

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        //get courses by category id and sort by rating
        [HttpGet("by-category-rating")]
        public async Task<ActionResult<GeneralResponse<PagedResult<GetCourseByCategoryIdAndRatingSortDto>>>>
        GetCoursesByCategoryAndRating([FromQuery] int? categoryId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _mediator.Send(new GetCoursesByCategoryIdAndRatingSortQuery(categoryId, pageNumber, pageSize));
            return Ok(result);
        }


        //get courses count
        [HttpGet("count")]
        public async Task<ActionResult<GeneralResponse<int>>> GetCoursesCount()
        {
            var result = await _mediator.Send(new CountCourseQuery());
            return Ok(result);
        }
        // Get Filtered Courses
        [HttpGet("Filter")]
        public async Task<ActionResult<GeneralResponse<PagedResult<FilterCourseDto>>>> GetFilteredCourses(
            [FromQuery] GetFilteredCoursesQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        [HttpGet("NumberOfCoursesPerCategory")]
        public async Task<ActionResult<GeneralResponse<PagedResult<NumberOfCoursesPerCategoryDTO>>>>
    GetNumberOfCoursesPerCategory([FromQuery] GetNumberOfCoursesPerCategoryQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
