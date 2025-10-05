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
    public class GetAllCoursesQuery : IRequest<GeneralResponse<PagedResult<GetAllCoursesDto>>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class GetAllCoursesQueryHandler
        : IRequestHandler<GetAllCoursesQuery, GeneralResponse<PagedResult<GetAllCoursesDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllCoursesQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GeneralResponse<PagedResult<GetAllCoursesDto>>> Handle(
            GetAllCoursesQuery request,
            CancellationToken cancellationToken)
        {
            var query = _unitOfWork.courseRepository
                .GetAllAsync(request.PageNumber, request.PageSize);

            var totalCount = await _unitOfWork.courseRepository.CountAsync();

            var courses = await query
                .Select(c => new GetAllCoursesDto
                {
                    Id = c.Id,
                    Title = c.Title,
                    Description = c.Description,
                    Certificate = c.Certificate,
                    InstructorName = c.Instructor.Name,
                    CategoryName = c.Category.Name,
                    Price = c.Price,
                    Level = c.Level,
                    Rating = c.Rating,
                    TotalHours = c.TotalHours,
                    ImageUrl = c.ImageUrl,
                    CategoryId = c.CategoryId,
                    InstructorId = c.InstructorId,
                    NumberOfLectures = c.Lectures.Count(l => !l.IsDeleted),
                    ReleaseDate=c.ReleaseDate
                })
                .ToListAsync(cancellationToken);

            var pagedResult = new PagedResult<GetAllCoursesDto>
            {
                Items = courses,
                TotalCount = totalCount,
                PageIndex = request.PageNumber,
                PageSize = request.PageSize
            };

            return GeneralResponse<PagedResult<GetAllCoursesDto>>.SuccessResponse("Courses retrieved successfully.", pagedResult);
        }
    }
}
