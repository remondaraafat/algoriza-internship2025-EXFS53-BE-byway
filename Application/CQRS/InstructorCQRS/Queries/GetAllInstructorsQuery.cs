using APICoursePlatform.Helpers;
using APICoursePlatform.UnitOfWorkContract;
using Application.DTOs.InstructorDTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.InstructorCQRS.Queries
{
    public class GetAllInstructorsQuery : IRequest<GeneralResponse<PagedResult<GetAllInstructorsDto>>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class GetAllInstructorsHandler : IRequestHandler<GetAllInstructorsQuery, GeneralResponse<PagedResult<GetAllInstructorsDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllInstructorsHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GeneralResponse<PagedResult<GetAllInstructorsDto>>> Handle(GetAllInstructorsQuery request, CancellationToken cancellationToken)
        {
            var query = _unitOfWork.InstructorRepository.GetAllAsync(request.PageNumber, request.PageSize);
            var items = query.Select(i => new GetAllInstructorsDto
            {
                Id = i.Id,
                Name = i.Name,
                JobTitle = i.jobTitle,
                Rating = i.Rating,
                Bio = i.Bio,
                ImageUrl= i.ImageUrl
            }).AsNoTracking().ToList();

            var totalCount = await _unitOfWork.InstructorRepository.CountAsync();

            var result = new PagedResult<GetAllInstructorsDto>
            {
                Items = items,
                TotalCount = totalCount,
                PageIndex = request.PageNumber,
                PageSize = request.PageSize
            };

            return GeneralResponse<PagedResult<GetAllInstructorsDto>>.SuccessResponse("Instructors fetched successfully", result);
        }
    }
}
