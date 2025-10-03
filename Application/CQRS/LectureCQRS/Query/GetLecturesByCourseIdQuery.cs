using APICoursePlatform.Helpers;
using APICoursePlatform.UnitOfWorkContract;
using Application.DTOs.LectureDTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.LectureCQRS.Query
{
    public class GetLecturesByCourseIdQuery : IRequest<GeneralResponse<PagedResult<GetLectureByCourseIdDto>>>
    {
        public int CourseId { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public GetLecturesByCourseIdQuery(int courseId, int pageNumber = 1, int pageSize = 10)
        {
            CourseId = courseId;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
    public class GetLecturesByCourseIdHandler : IRequestHandler<GetLecturesByCourseIdQuery, GeneralResponse<PagedResult<GetLectureByCourseIdDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetLecturesByCourseIdHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GeneralResponse<PagedResult<GetLectureByCourseIdDto>>> Handle(GetLecturesByCourseIdQuery request, CancellationToken cancellationToken)
        {
            // get total count
            var totalCount = await _unitOfWork.lectureRepository.CountAsync(l => l.CourseId == request.CourseId);

            // apply pagination
            var lectures = _unitOfWork.lectureRepository
                .GetWithFilterAsync(l => l.CourseId == request.CourseId, request.PageNumber, request.PageSize)
                .OrderBy(l => l.Order)
                .Select(l => new GetLectureByCourseIdDto
                {
                    Id = l.Id,
                    Title = l.Title,
                    Order = l.Order,
                    DurationMinutes = l.DurationMinutes,
                    CourseId = l.CourseId
                })
                .ToList();

            var pagedResult = new PagedResult<GetLectureByCourseIdDto>
            {
                Items = lectures,
                TotalCount = totalCount,
                PageIndex = request.PageNumber,
                PageSize = request.PageSize
            };

            return GeneralResponse<PagedResult<GetLectureByCourseIdDto>>.SuccessResponse("Lectures retrieved successfully", pagedResult);
        }
    }
}
