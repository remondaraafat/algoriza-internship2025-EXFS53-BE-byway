using APICoursePlatform.Helpers;
using APICoursePlatform.UnitOfWorkContract;
using Application.DTOs.PaymentCourseDTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.CoursePaymentCQRS.Command
{
    public class CreatePaymentCourseCommand : IRequest<GeneralResponse<PaymentCourse>>
    {
        public CreatePaymentCourseDto PaymentCourseDto { get; set; }

        public CreatePaymentCourseCommand(CreatePaymentCourseDto dto)
        {
            PaymentCourseDto = dto;
        }
    }

    // 2. Handler
    public class CreatePaymentCourseCommandHandler : IRequestHandler<CreatePaymentCourseCommand, GeneralResponse<PaymentCourse>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreatePaymentCourseCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GeneralResponse<PaymentCourse>> Handle(CreatePaymentCourseCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Validate if combination already exists (optional)
                bool exists = await _unitOfWork.paymentCourseRepository.ExistsAsync(
                    pc => pc.PaymentId == request.PaymentCourseDto.PaymentId &&
                          pc.CourseId == request.PaymentCourseDto.CourseId
                );

                if (exists)
                    return GeneralResponse<PaymentCourse>.FailResponse("This payment is already linked to this course.");

                // Map DTO to entity
                var paymentCourse = new PaymentCourse
                {
                    PaymentId = request.PaymentCourseDto.PaymentId,
                    CourseId = request.PaymentCourseDto.CourseId
                };

                // Add and commit
                await _unitOfWork.paymentCourseRepository.AddAsync(paymentCourse);
                await _unitOfWork.SaveAsync();

                return GeneralResponse<PaymentCourse>.SuccessResponse("PaymentCourse created successfully.", paymentCourse);
            }
            catch (Exception ex)
            {
                return GeneralResponse<PaymentCourse>.FailResponse($"Error creating PaymentCourse: {ex.Message}");
            }
        }
    }
}
