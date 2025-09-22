using Application.CQRS.CourseCQRS.Command;
using Application.DTOs.CourseDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Application.CQRS.CourseCQRS.Command;
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

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateCourse([FromForm] CreateCourseDto dto)
        {
            var result = await _mediator.Send(new CreateCourseCommand{CourseDto=dto});
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
        [HttpPut("update")]
        public async Task<IActionResult> UpdateCourse([FromForm] UpdateCourseDto dto)
        {
            var response = await _mediator.Send(new UpdateCourseCommand { Dto=dto});
            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }
    }
}
