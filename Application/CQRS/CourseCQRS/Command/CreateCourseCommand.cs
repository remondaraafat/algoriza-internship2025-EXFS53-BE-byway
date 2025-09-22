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
    public class CreateCourseCommand : IRequest<GeneralResponse<int>>
    {
        public CreateCourseDto CourseDto { get; set; }

       
    }
    public class CreateCourseCommandHandler : IRequestHandler<CreateCourseCommand, GeneralResponse<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        

        public CreateCourseCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
           
        }

        public async Task<GeneralResponse<int>> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
        {
            var dto = request.CourseDto;

            //FileService
            var uploadResult = await FileService.UploadFileAsync(dto.ImageFile);

            if (!uploadResult.Success)
            {
                return GeneralResponse<int>.FailResponse($"Image upload failed: {uploadResult.Message}", 0);
            }

           


            // obj
            var course = new Course
            {
                Title = dto.Title,
                Description = dto.Description,
                Certificate = dto.Certificate,
                Price = dto.Price,
                Level = dto.Level,
                Rating = dto.Rating,
                ReleaseDate = DateTime.Now,
                TotalHours = dto.TotalHours,
                ImageUrl = uploadResult.Data,
                CategoryId = dto.CategoryId,
                InstructorId = dto.InstructorId
            };

            try
            {
                await _unitOfWork.courseRepository.AddAsync(course);
                await _unitOfWork.SaveAsync();

                return GeneralResponse<int>.SuccessResponse("Course created successfully", course.Id);
            }
            catch (Exception ex)
            {
                return GeneralResponse<int>.FailResponse($"Error creating course: {ex.Message}");
            }
        }
    }
}
