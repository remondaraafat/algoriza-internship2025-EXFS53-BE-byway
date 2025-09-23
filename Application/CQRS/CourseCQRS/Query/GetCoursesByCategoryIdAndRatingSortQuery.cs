using APICoursePlatform.Helpers;
using APICoursePlatform.UnitOfWorkContract;
using Application.DTOs.CourseDTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.CourseCQRS.Query
{
    public class GetCoursesByCategoryIdAndRatingSortQuery
        : IRequest<GeneralResponse<PagedResult<GetCourseByCategoryIdAndRatingSortDto>>>
    {
        public int? CategoryId { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public GetCoursesByCategoryIdAndRatingSortQuery(int? categoryId, int pageNumber = 1, int pageSize = 10)
        {
            CategoryId = categoryId;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }

    public class GetCoursesByCategoryIdAndRatingSortQueryHandler
        : IRequestHandler<GetCoursesByCategoryIdAndRatingSortQuery, GeneralResponse<PagedResult<GetCourseByCategoryIdAndRatingSortDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetCoursesByCategoryIdAndRatingSortQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GeneralResponse<PagedResult<GetCourseByCategoryIdAndRatingSortDto>>> Handle(
            GetCoursesByCategoryIdAndRatingSortQuery request,
            CancellationToken cancellationToken)
        {
            IQueryable<Course> coursesQuery;

            if (request.CategoryId.HasValue && request.CategoryId.Value > 0)
            {
                coursesQuery = _unitOfWork.courseRepository
                    .GetWithFilterAsync(c => c.CategoryId == request.CategoryId.Value, request.PageNumber, request.PageSize);
            }
            else
            {
                coursesQuery = _unitOfWork.courseRepository
                    .GetAllAsync(request.PageNumber, request.PageSize);
            }

            var totalCount = await coursesQuery.CountAsync(cancellationToken);

            var courses = await coursesQuery
                .OrderByDescending(c => c.Rating)
                .Select(c => new GetCourseByCategoryIdAndRatingSortDto
                {
                    Id = c.Id,
                    Title = c.Title,
                    Description = c.Description,
                    Certificate = c.Certificate,
                    Price = c.Price,
                    Level = c.Level,
                    Rating = c.Rating,
                    NumberOfLectures = c.Lectures.Count(l => !l.IsDeleted),
                    TotalHours = c.TotalHours,
                    ImageUrl = c.ImageUrl,
                    CategoryId = c.CategoryId,
                    InstructorId = c.InstructorId,
                    ReleaseDate = c.ReleaseDate
                })
                .ToListAsync(cancellationToken);

            var pagedResult = new PagedResult<GetCourseByCategoryIdAndRatingSortDto>
            {
                Items = courses,
                TotalCount = totalCount,
                PageIndex = request.PageNumber,
                PageSize = request.PageSize
            };

            return GeneralResponse<PagedResult<GetCourseByCategoryIdAndRatingSortDto>>.SuccessResponse("Courses retrieved successfully.", pagedResult);
        }
    }
}
