using APICoursePlatform.Helpers;
using APICoursePlatform.UnitOfWorkContract;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.InstructorCQRS.Queries
{
    public class GetInstructorCountQuery : IRequest<GeneralResponse<int>> { }

    public class GetInstructorCountHandler : IRequestHandler<GetInstructorCountQuery, GeneralResponse<int>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetInstructorCountHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GeneralResponse<int>> Handle(GetInstructorCountQuery request, CancellationToken cancellationToken)
        {
            var count = await _unitOfWork.InstructorRepository.CountAsync();
                            

            return GeneralResponse<int>.SuccessResponse("Instructor count fetched successfully", count);
        }
    }
}
