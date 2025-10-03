using APICoursePlatform.Helpers;
using APICoursePlatform.UnitOfWorkContract;
using Application.DTOs.CourseDTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.CourseCQRS.Query
{
    // Query request
    public class GetNumberOfCoursesPerCategoryQuery
        : IRequest<GeneralResponse<PagedResult<NumberOfCoursesPerCategoryDTO>>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class GetNumberOfCoursesPerCategoryQueryHandler
        : IRequestHandler<GetNumberOfCoursesPerCategoryQuery, GeneralResponse<PagedResult<NumberOfCoursesPerCategoryDTO>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetNumberOfCoursesPerCategoryQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GeneralResponse<PagedResult<NumberOfCoursesPerCategoryDTO>>> Handle(
            GetNumberOfCoursesPerCategoryQuery request,
            CancellationToken cancellationToken)
        {
            try {
                // get paginated categories
                var items = await _unitOfWork.categoryRepository
                    .GetAllAsync(request.PageNumber, request.PageSize)
                    .Select(c => new NumberOfCoursesPerCategoryDTO
                    {
                        CategoryID = c.Id,
                        CategoryName = c.Name,
                        CategoryImage= c.ImageUrl,
                        NumberOfCourses = c.Courses.Count(course => !course.IsDeleted)
                    }).ToListAsync(cancellationToken);

                // total count (without pagination)
                var totalCount = await _unitOfWork.categoryRepository.CountAsync();

                var pagedResult = new PagedResult<NumberOfCoursesPerCategoryDTO>
                {
                    Items = items,
                    TotalCount = totalCount,
                    PageIndex = request.PageNumber,
                    PageSize = request.PageSize
                };

                return GeneralResponse<PagedResult<NumberOfCoursesPerCategoryDTO>>
                    .SuccessResponse("Retrieved number of courses per category successfully", pagedResult);
            }
            catch (Exception ex)
            {
                return GeneralResponse<PagedResult<NumberOfCoursesPerCategoryDTO>>
                    .FailResponse($"An error occurred: {ex.Message}");
            }
            
        }
    }
}
