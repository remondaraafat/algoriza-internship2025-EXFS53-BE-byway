using APICoursePlatform.Helpers;
using APICoursePlatform.UnitOfWorkContract;
using Application.DTOs.CourseDTOs;
using Application.Servicies;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.CourseCQRS.Command
{
    public class UpdateCourseCommand : IRequest<GeneralResponse<string>>
    {
        public UpdateCourseDto Dto { get; set; }

       
    }
    public class UpdateCourseCommandHandler : IRequestHandler<UpdateCourseCommand, GeneralResponse<string>>
    {
        private readonly IUnitOfWork _unitOfWork;
       
        public UpdateCourseCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            
        }

        public async Task<GeneralResponse<string>> Handle(UpdateCourseCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Dto;

            // Check if course exists
            var course = await _unitOfWork.courseRepository.GetByIdAsync(dto.Id);
            if (course == null)
                return GeneralResponse<string>.FailResponse("Course not found");

            // Upload image
            var uploadResult = await FileService.UploadFileAsync(dto.ImageFile);

            if (!uploadResult.Success)
            {
                return GeneralResponse<string>.FailResponse($"Image upload failed: {uploadResult.Message}", request.Dto.Title);
            }

            // Update entity
            course.Title = dto.Title;
            course.Description = dto.Description;
            course.Certificate = dto.Certificate;
            course.Price = dto.Price;
            course.Level = dto.Level;
            course.Rating = dto.Rating;
            course.TotalHours = dto.TotalHours;
            course.ImageUrl = uploadResult.Data;
            course.CategoryId = dto.CategoryId;
            course.InstructorId = dto.InstructorId;

            _unitOfWork.courseRepository.Update(course);
            await _unitOfWork.SaveAsync();

            return GeneralResponse<string>.SuccessResponse("Course updated successfully", course.Title);
        }
    }
}
