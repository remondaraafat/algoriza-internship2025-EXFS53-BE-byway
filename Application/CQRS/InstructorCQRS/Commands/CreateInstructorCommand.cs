using APICoursePlatform.Helpers;
using APICoursePlatform.UnitOfWorkContract;
using Application.DTOs.InstructorDTOs;
using Application.Servicies;
using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.InstructorCQRS.Commands
{
    public class CreateInstructorCommand : IRequest<GeneralResponse<int>>
    {
        public CreateInstructorDto DTO { get; set; }
    }

    public class CreateInstructorHandler : IRequestHandler<CreateInstructorCommand, GeneralResponse<int>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateInstructorHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GeneralResponse<int>> Handle(CreateInstructorCommand request, CancellationToken cancellationToken)
        {
            var dto = request.DTO;
            var imagePath = await FileService.UploadFileAsync(request.DTO.ImageFile);
            if (imagePath.StartsWith("Error") || imagePath.StartsWith("File"))
                return GeneralResponse<int>.FailResponse("Image upload failed", 0);

            var instructor = new Instructor
            {
                Name = dto.Name,
                Bio = dto.Bio,
                jobTitle = dto.JobTitle,
                Rating = dto.Rating,
                ImageUrl = imagePath
            };

            await _unitOfWork.InstructorRepository.AddAsync(instructor);
            await _unitOfWork.SaveAsync();

            return GeneralResponse<int>.SuccessResponse("Instructor created successfully", instructor.Id);
        }
    }
}
