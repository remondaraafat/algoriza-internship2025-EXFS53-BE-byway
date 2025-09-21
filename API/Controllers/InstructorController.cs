using Application.CQRS.InstructorCQRS.Commands;
using Application.CQRS.InstructorCQRS.Queries;
using Application.DTOs.InstructorDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static APICoursePlatform.Enums.Enums;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstructorController : ControllerBase
    {
        private readonly IMediator _mediator;

        public InstructorController(IMediator mediator)
        {
            _mediator = mediator;
        }
        // 1️⃣ Get All Instructors (Paged)
        [HttpGet("GetAll")]
        public async Task<ActionResult<GeneralResponse<PagedResult<GetAllInstructorsDto>>>> GetAll(int pageNumber = 1, int pageSize = 10)
        {
            var query = new GetAllInstructorsQuery { PageNumber = pageNumber, PageSize = pageSize };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        // 2️⃣ Get Instructor By Id
        [HttpGet("{id}")]
        public async Task<ActionResult<GeneralResponse<GetOneInstructorDto>>> GetById(int id)
        {
            var query = new GetInstructorByIdQuery { Id = id };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        // 3️⃣ Create Instructor
        [HttpPost("Create")]
        public async Task<ActionResult<GeneralResponse<int>>> Create([FromForm] CreateInstructorDto dto)
        {
            var command = new CreateInstructorCommand { DTO = dto };
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        // 4️⃣ Update Instructor
        [HttpPut("Update/{id}")]
        public async Task<ActionResult<GeneralResponse<int>>> Update(int id, [FromForm] UpdateInstructorDto dto)
        {
            dto.Id = id;
            var command = new UpdateInstructorCommand { DTO = dto };
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        // 5️⃣ Delete Instructor
        [HttpDelete("{id}")]
        public async Task<ActionResult<GeneralResponse<int>>> Delete(int id)
        {
            var command = new DeleteInstructorCommand { Id = id };
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        // 6️⃣ Get Instructor Names
        [HttpGet("Names")]
        public async Task<ActionResult<GeneralResponse<List<GetInstructorNamesDto>>>> GetInstructorNames()
        {
            var query = new GetInstructorNamesQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        // 7️⃣ Filter Instructors By Name and JobTitle
        [HttpGet("Filter")]
        public async Task<ActionResult<GeneralResponse<PagedResult<FilterInstructorDto>>>> Filter([FromQuery] string name, [FromQuery] int? jobTitle, int pageNumber = 1, int pageSize = 10)
        {
            var query = new FilterInstructorsQuery
            {
                Name = name,
                JobTitle = jobTitle.HasValue ? (JobTitle?)jobTitle.Value : null,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var result = await _mediator.Send(query);
            return Ok(result);
        }

        // 8️⃣ Get Instructor Count
        [HttpGet("Count")]
        public async Task<ActionResult<GeneralResponse<int>>> GetCount()
        {
            var query = new GetInstructorCountQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}

