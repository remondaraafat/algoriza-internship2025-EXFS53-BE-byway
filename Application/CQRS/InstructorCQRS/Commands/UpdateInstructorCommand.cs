using APICoursePlatform.Helpers;
using APICoursePlatform.UnitOfWorkContract;
using Application.DTOs.InstructorDTOs;
using Application.Servicies;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.InstructorCQRS.Commands
{
    public class UpdateInstructorCommand : IRequest<GeneralResponse<bool>>
    {
        public UpdateInstructorDto DTO { get; set; }
    }

    public class UpdateInstructorHandler : IRequestHandler<UpdateInstructorCommand, GeneralResponse<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        

        public UpdateInstructorHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
           
        }

        public async Task<GeneralResponse<bool>> Handle(UpdateInstructorCommand request, CancellationToken cancellationToken)
        {
            var dto = request.DTO;
            var instructor = await _unitOfWork.InstructorRepository.GetByIdAsync(dto.Id);

            if (instructor == null)
                return GeneralResponse<bool>.FailResponse("Instructor not found");

            instructor.Name = dto.Name;
            instructor.Bio = dto.Bio;
            instructor.jobTitle = dto.JobTitle;
            instructor.Rating = dto.Rating;

            
            
                // FileService call
                var uploadResult = await FileService.UploadFileAsync(dto.ImageFile);

                if (!uploadResult.Success)
                {
                    return GeneralResponse<bool>.FailResponse($"Image upload failed: {uploadResult.Message}", false);
                }

                instructor.ImageUrl = uploadResult.Data;
            

            _unitOfWork.InstructorRepository.Update(instructor);
            await _unitOfWork.SaveAsync();

            return GeneralResponse<bool>.SuccessResponse("Instructor updated successfully", true);
        }
    }
}
