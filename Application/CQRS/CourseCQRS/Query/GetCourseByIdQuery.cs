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
    public class GetCourseByIdQuery : IRequest<GeneralResponse<GetCourseByIdDto>>
    {
        public int Id { get; set; }

        public string UserId { get; set; }
    }

    public class GetCourseByIdQueryHandler : IRequestHandler<GetCourseByIdQuery, GeneralResponse<GetCourseByIdDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetCourseByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GeneralResponse<GetCourseByIdDto>> Handle(GetCourseByIdQuery request, CancellationToken cancellationToken)
        {
           

            var course = await _unitOfWork.courseRepository
                .GetFirstOrDefaultAsync(c => c.Id == request.Id);

            if (course == null)
                return GeneralResponse<GetCourseByIdDto>.FailResponse("Course not found");

            bool isBought = false;
            bool isInCart = false;
            if (request.UserId != null)
            {
                // Check if course already purchased by user
                 isBought = await _unitOfWork.paymentCourseRepository
                    .ExistsAsync(pc => pc.CourseId == request.Id && pc.Payment.UserId == request.UserId);


                // Check if course already in user's cart
                 isInCart = await _unitOfWork.CartItemRepository
                    .ExistsAsync(c => c.CourseId == request.Id && c.UserId == request.UserId);
            }



                var dto = new GetCourseByIdDto
                {
                    Id = course.Id,
                    Title = course.Title,
                    Description = course.Description,
                    Certificate = course.Certificate,
                    InstructorName = course.Instructor?.Name,
                    InstructorImage= course.Instructor?.ImageUrl,
                    CategoryName = course.Category?.Name,
                    Price = course.Price,
                    Level = course.Level,
                    Rating = course.Rating,
                    NumberOfLectures = course.Lectures.Count(l => !l.IsDeleted),
                    TotalHours = course.TotalHours,
                    ImageUrl = course.ImageUrl,
                    CategoryId = course.CategoryId,
                    InstructorId = course.InstructorId,
                    ReleaseDate = course.ReleaseDate,
                    IsBought = isBought,
                    IsInCart = isInCart

                };

            return GeneralResponse<GetCourseByIdDto>.SuccessResponse("Course retrieved successfully", dto);
        }
    }
}
