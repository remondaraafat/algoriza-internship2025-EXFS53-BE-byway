using Application.CQRS.LectureCQRS.Command;
using Application.CQRS.LectureCQRS.Query;
using Application.DTOs.LectureDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LectureController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LectureController(IMediator mediator)
        {
            _mediator = mediator;
        }
        //create lecture
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<GeneralResponse<CreateLectuerDto>>> CreateLecture([FromBody] CreateLectuerDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(GeneralResponse<CreateLectuerDto>.FailResponse("Validation failed", dto));

            var command = new CreateLectureCommand(dto);
            var result = await _mediator.Send(command);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
        // update lecture
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult<GeneralResponse<UpdateLectureDto>>> UpdateLecture(
            int id,
            [FromBody] UpdateLectureDto dto)
        {
            if (id != dto.Id)
                return BadRequest(GeneralResponse<UpdateLectureDto>.FailResponse("Lecture ID mismatch"));

            var result = await _mediator.Send(new UpdateLectureCommand(dto));

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }
        // Delete lecture
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<GeneralResponse<string>>> DeleteLecture(int id)
        {
            var result = await _mediator.Send(new DeleteLectureCommand(id));

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }
        // Get lectures by course id with pagination
        [HttpGet("Course/{courseId}")]
        public async Task<ActionResult<GeneralResponse<PagedResult<GetLectureByCourseIdDto>>>> GetLecturesByCourseId(
            int courseId,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = new GetLecturesByCourseIdQuery(courseId, pageNumber, pageSize);
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
