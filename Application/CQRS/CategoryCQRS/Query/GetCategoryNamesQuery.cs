using APICoursePlatform.Helpers;
using APICoursePlatform.UnitOfWorkContract;
using Application.DTOs.CategoryDTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.CategoryCQRS.Query
{
    public class GetCategoryNamesQuery : IRequest<GeneralResponse<List<GetCategoryNamesDto>>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
    public class GetCategoryNamesQueryHandler : IRequestHandler<GetCategoryNamesQuery, GeneralResponse<List<GetCategoryNamesDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetCategoryNamesQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GeneralResponse<List<GetCategoryNamesDto>>> Handle(GetCategoryNamesQuery request, CancellationToken cancellationToken)
        {
            var query = _unitOfWork.categoryRepository
                                   .GetAllAsync(request.PageNumber, request.PageSize);

            var categories = await query
                .Select(c => new GetCategoryNamesDto
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToListAsync(cancellationToken);

            return GeneralResponse<List<GetCategoryNamesDto>>.SuccessResponse(
                "Categories fetched successfully",
                categories
            );
        }
    }
}
