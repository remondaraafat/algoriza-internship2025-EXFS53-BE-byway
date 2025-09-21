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
    public class GetInstructorNamesQuery : IRequest<GeneralResponse<List<GetInstructorNamesDto>>> { }

    public class GetInstructorNamesHandler : IRequestHandler<GetInstructorNamesQuery, GeneralResponse<List<GetInstructorNamesDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetInstructorNamesHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GeneralResponse<List<GetInstructorNamesDto>>> Handle(GetInstructorNamesQuery request, CancellationToken cancellationToken)
        {
            var items = _unitOfWork.InstructorRepository.GetAllAsync()
                .Select(i => new GetInstructorNamesDto { Id = i.Id, Name = i.Name })
                .ToList();

            return GeneralResponse<List<GetInstructorNamesDto>>.SuccessResponse("Instructor names fetched successfully", items);
        }
    }
}
