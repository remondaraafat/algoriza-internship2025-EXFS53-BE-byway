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
    public class UpdateLectureCommand : IRequest<GeneralResponse<UpdateLectureDto>>
    {
        public UpdateLectureDto Dto { get; }

        public UpdateLectureCommand(UpdateLectureDto dto)
        {
            Dto = dto;
        }
    }

    public class UpdateLectureCommandHandler : IRequestHandler<UpdateLectureCommand, GeneralResponse<UpdateLectureDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateLectureCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GeneralResponse<UpdateLectureDto>> Handle(UpdateLectureCommand request, CancellationToken cancellationToken)
        {
            try {
                var dto = request.Dto;

                // Find lecture
                var lecture = await _unitOfWork.lectureRepository.GetByIdAsync(dto.Id);
                if (lecture == null)
                    return GeneralResponse<UpdateLectureDto>.FailResponse("Lecture not found");

                // Update values
                lecture.Title = dto.Title;
                lecture.Order = dto.Order;
                lecture.DurationMinutes = dto.DurationMinutes;
                lecture.CourseId = dto.CourseId;

                _unitOfWork.lectureRepository.Update(lecture);
                await _unitOfWork.SaveAsync();

                return GeneralResponse<UpdateLectureDto>.SuccessResponse("Lecture updated successfully", dto);
            }
            catch (Exception ex)
            {
                return GeneralResponse<UpdateLectureDto>.FailResponse($"Error: {ex.Message}");
            }
           
        }
    }
}
