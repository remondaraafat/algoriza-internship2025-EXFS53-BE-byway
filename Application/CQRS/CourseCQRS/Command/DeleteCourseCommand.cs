using APICoursePlatform.Helpers;
using APICoursePlatform.UnitOfWorkContract;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.CourseCQRS.Command
{
    public class DeleteCourseCommand : IRequest<GeneralResponse<string>>
    {
        public int Id { get; set; }

    }

    public class DeleteCourseCommandHandler : IRequestHandler<DeleteCourseCommand, GeneralResponse<string>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCourseCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GeneralResponse<string>> Handle(DeleteCourseCommand request, CancellationToken cancellationToken)
        {

            try {
                var course = await _unitOfWork.courseRepository.GetByIdAsync(request.Id);
                if (course == null)
                {
                    return GeneralResponse<string>.FailResponse("Course not found.");
                }
                // Check if course has enrolled users in PaymentCourse
                var hasEnrolledUsers = await _unitOfWork.paymentCourseRepository
                    .GetWithFilterAsync(pc => pc.CourseId == request.Id)
                    .AnyAsync(cancellationToken);

                if (hasEnrolledUsers)
                    return GeneralResponse<string>.FailResponse("Cannot delete course. Users are already enrolled.");

                await _unitOfWork.courseRepository.DeleteAsync(c => c.Id == request.Id);
                await _unitOfWork.SaveAsync();

                return GeneralResponse<string>.SuccessResponse("Course deleted successfully.");
            }
            catch (Exception ex)
            {
                return GeneralResponse<string>.FailResponse($"An error occurred while deleting the course: {ex.Message}");
            }
        }
    }
}
