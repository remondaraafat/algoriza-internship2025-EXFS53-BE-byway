using APICoursePlatform.Helpers;
using APICoursePlatform.UnitOfWorkContract;
using Application.DTOs.InstructorDTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Application.CQRS.InstructorCQRS.Queries
{
    public class GetInstructorNamesQuery : IRequest<GeneralResponse<PagedResult<GetInstructorNamesDto>>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class GetInstructorNamesHandler : IRequestHandler<GetInstructorNamesQuery, GeneralResponse<PagedResult<GetInstructorNamesDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetInstructorNamesHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GeneralResponse<PagedResult<GetInstructorNamesDto>>> Handle(GetInstructorNamesQuery request, CancellationToken cancellationToken)
        {

            var items = await _unitOfWork.InstructorRepository.GetAllAsync(request.PageNumber, request.PageSize)
                .Select(i => new GetInstructorNamesDto
                {
                    Id = i.Id,
                    Name = i.Name
                })
                .ToListAsync(cancellationToken);

            var totalCount = await _unitOfWork.InstructorRepository.CountAsync();

            var pagedResult = new PagedResult<GetInstructorNamesDto>
            {
                Items = items,
                TotalCount = totalCount,
                PageIndex = request.PageNumber,
                PageSize = request.PageSize
            };

            return GeneralResponse<PagedResult<GetInstructorNamesDto>>.SuccessResponse("Instructor names fetched successfully", pagedResult);
        }
    }
}
