using APICoursePlatform.Helpers;
using APICoursePlatform.UnitOfWorkContract;
using Application.DTOs.LectureDTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.LectureCQRS.Command
{
    public class CreateLectureCommand : IRequest<GeneralResponse<CreateLectuerDto>>
    {
        public CreateLectuerDto Dto { get; set; }

        public CreateLectureCommand(CreateLectuerDto dto)
        {
            Dto = dto;
        }
    }

    public class CreateLectureCommandHandler : IRequestHandler<CreateLectureCommand, GeneralResponse<CreateLectuerDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateLectureCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GeneralResponse<CreateLectuerDto>> Handle(CreateLectureCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Check if course exists
                var course = await _unitOfWork.courseRepository.GetByIdAsync(request.Dto.CourseId);
                if (course == null)
                    return GeneralResponse<CreateLectuerDto>.FailResponse("Course not found");

                // Map DTO → Entity
                var lecture = new Lecture
                {
                    Title = request.Dto.Title,
                    Order = request.Dto.Order,
                    DurationMinutes = request.Dto.DurationMinutes,
                    CourseId = request.Dto.CourseId
                };

                // Save entity
                await _unitOfWork.lectureRepository.AddAsync(lecture);
                await _unitOfWork.SaveAsync();

                return GeneralResponse<CreateLectuerDto>.SuccessResponse("Lecture created successfully", request.Dto);
            }
            catch (System.Exception ex)
            {
                return GeneralResponse<CreateLectuerDto>.FailResponse($"Error: {ex.Message}");
            }
        }
    }
}
