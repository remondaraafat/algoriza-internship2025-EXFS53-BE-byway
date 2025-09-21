using APICoursePlatform.Helpers;
using APICoursePlatform.UnitOfWorkContract;
using Application.DTOs.InstructorDTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.InstructorCQRS.Queries
{
    public class GetInstructorByIdQuery : IRequest<GeneralResponse<GetOneInstructorDto>>
    {
        public int Id { get; set; }
    }

    public class GetInstructorByIdHandler : IRequestHandler<GetInstructorByIdQuery, GeneralResponse<GetOneInstructorDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetInstructorByIdHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GeneralResponse<GetOneInstructorDto>> Handle(GetInstructorByIdQuery request, CancellationToken cancellationToken)
        {
            var instructor = await _unitOfWork.InstructorRepository.GetByIdAsync(request.Id);
            if (instructor == null)
                return GeneralResponse<GetOneInstructorDto>.FailResponse("Instructor not found");

            var dto = new GetOneInstructorDto
            {
                Id = instructor.Id,
                Name = instructor.Name,
                Bio = instructor.Bio,
                JobTitle = instructor.jobTitle,
                Rating = instructor.Rating,
                ImageUrl = instructor.ImageUrl
            };

            return GeneralResponse<GetOneInstructorDto>.SuccessResponse("Instructor fetched successfully", dto);
        }
    }
}
