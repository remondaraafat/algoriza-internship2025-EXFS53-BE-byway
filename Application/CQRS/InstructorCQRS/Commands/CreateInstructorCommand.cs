using APICoursePlatform.Helpers;
using APICoursePlatform.UnitOfWorkContract;
using Application.DTOs.InstructorDTOs;
using Application.Servicies;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
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
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CreateInstructorHandler(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<GeneralResponse<int>> Handle(CreateInstructorCommand request, CancellationToken cancellationToken)
        {
            var dto = request.DTO;
            // FileService call
            var uploadResult = await FileService.UploadFileAsync(dto.ImageFile);

            if (!uploadResult.Success)
            {
                return GeneralResponse<int>.FailResponse($"Image upload failed: {uploadResult.Message}",0);
            }

          


            var instructor = new Instructor
            {
                Name = dto.Name,
                Bio = dto.Bio,
                jobTitle = dto.JobTitle,
                Rating = dto.Rating,
                ImageUrl = uploadResult.Data
            };

            await _unitOfWork.InstructorRepository.AddAsync(instructor);
            await _unitOfWork.SaveAsync();

            return GeneralResponse<int>.SuccessResponse("Instructor created successfully", instructor.Id);
        }
    }
}
