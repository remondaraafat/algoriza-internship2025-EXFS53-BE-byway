using APICoursePlatform.Helpers;
using APICoursePlatform.UnitOfWorkContract;
using Application.DTOs.CourseDTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static APICoursePlatform.Enums.Enums;

namespace Application.CQRS.CourseCQRS.Query
{
    public class GetFilteredCoursesQuery : IRequest<GeneralResponse<PagedResult<FilterCourseDto>>>
    {
        public string? Name { get; set; }
        public int? Rating { get; set; }
        public int? MinLectures { get; set; }
        public int? MaxLectures { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public int? CategoryId { get; set; }

        // Now it's an Enum
        public CourseSortBy SortBy { get; set; } = CourseSortBy.Latest;

        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
    public class GetFilteredCoursesQueryHandler
        : IRequestHandler<GetFilteredCoursesQuery, GeneralResponse<PagedResult<FilterCourseDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetFilteredCoursesQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GeneralResponse<PagedResult<FilterCourseDto>>> Handle(
            GetFilteredCoursesQuery request,
            CancellationToken cancellationToken)
        {
            try {
                var query = _unitOfWork.courseRepository.GetQueryable();

                // Filtering
                if (!string.IsNullOrEmpty(request.Name))
                    query = query.Where(c => c.Title.Contains(request.Name));

                if (request.Rating.HasValue)
                    query = query.Where(c => c.Rating == request.Rating.Value);

                if (request.MinLectures.HasValue)
                    query = query.Where(c => c.Lectures.Count(l => !l.IsDeleted) >= request.MinLectures.Value);

                if (request.MaxLectures.HasValue)
                    query = query.Where(c => c.Lectures.Count(l => !l.IsDeleted) <= request.MaxLectures.Value);

                if (request.MinPrice.HasValue)
                    query = query.Where(c => c.Price >= request.MinPrice.Value);

                if (request.MaxPrice.HasValue)
                    query = query.Where(c => c.Price <= request.MaxPrice.Value);

                if (request.CategoryId.HasValue)
                    query = query.Where(c => c.CategoryId == request.CategoryId.Value);

                // Sorting
                query = request.SortBy switch
                {
                    CourseSortBy.Latest => query.OrderByDescending(c => c.ReleaseDate),
                    CourseSortBy.Oldest => query.OrderBy(c => c.ReleaseDate),
                    CourseSortBy.HighestPrice => query.OrderByDescending(c => c.Price),
                    CourseSortBy.LowestPrice => query.OrderBy(c => c.Price),
                    _ => query.OrderByDescending(c => c.ReleaseDate) // default
                };

                // Count before pagination
                var totalCount = await query.CountAsync(cancellationToken);

                // Pagination
                var items = await query
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .Select(c => new FilterCourseDto
                    {
                        Id = c.Id,
                        Title = c.Title,
                        Description = c.Description,
                        Certificate = c.Certificate,
                        InstructorName = c.Instructor.Name,
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

                // paged result
                var pagedResult = new PagedResult<FilterCourseDto>
                {
                    Items = items,
                    TotalCount = totalCount,
                    PageIndex = request.PageNumber,
                    PageSize = request.PageSize
                };

                return GeneralResponse<PagedResult<FilterCourseDto>>.SuccessResponse("Courses retrieved successfully", pagedResult);

            } catch (Exception ex)
            {
                // Log the exception (ex) here if needed
                return GeneralResponse<PagedResult<FilterCourseDto>>.FailResponse("An error occurred while processing the request.");
            }
            
        }
    }
}
