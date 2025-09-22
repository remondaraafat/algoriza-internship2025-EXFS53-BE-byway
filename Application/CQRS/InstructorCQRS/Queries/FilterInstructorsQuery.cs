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
using static APICoursePlatform.Enums.Enums;

namespace Application.CQRS.InstructorCQRS.Queries
{
    public class FilterInstructorsQuery : IRequest<GeneralResponse<PagedResult<FilterInstructorDto>>>
    {
        public string Name { get; set; }
        public JobTitle? JobTitle { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class FilterInstructorsHandler : IRequestHandler<FilterInstructorsQuery, GeneralResponse<PagedResult<FilterInstructorDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public FilterInstructorsHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GeneralResponse<PagedResult<FilterInstructorDto>>> Handle(FilterInstructorsQuery request, CancellationToken cancellationToken)
        {
            var query = _unitOfWork.InstructorRepository.GetWithFilterAsync(
                i => (string.IsNullOrEmpty(request.Name) || i.Name.Contains(request.Name)) &&
                     (request.JobTitle == null || i.jobTitle == request.JobTitle),
                request.PageNumber, request.PageSize);

            var items = query.Select(i => new FilterInstructorDto
            {
                Id = i.Id,
                Name = i.Name,
                JobTitle = i.jobTitle,
                Rating = i.Rating,
                Bio = i.Bio,
                ImageUrl = i.ImageUrl
            }).AsNoTracking().ToList();

            var totalCount = await _unitOfWork.InstructorRepository.CountAsync(
                i => (string.IsNullOrEmpty(request.Name) || i.Name.Contains(request.Name)) &&
                     (request.JobTitle == null || i.jobTitle == request.JobTitle));

            var result = new PagedResult<FilterInstructorDto>
            {
                Items = items,
                TotalCount = totalCount,
                PageIndex = request.PageNumber,
                PageSize = request.PageSize
            };

            return GeneralResponse<PagedResult<FilterInstructorDto>>.SuccessResponse("Instructors fetched successfully", result);
        }
    }
}
