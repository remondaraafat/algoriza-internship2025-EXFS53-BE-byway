using APICoursePlatform.Helpers;
using APICoursePlatform.UnitOfWorkContract;
using Application.DTOs.InstructorDTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.InstructorCQRS.Commands
{
    public class DeleteInstructorCommand : IRequest<GeneralResponse<bool>>
    {
        [Required(ErrorMessage = "Instructor Id is required.")]
        public int Id { get; set; }
    }

    public class DeleteInstructorHandler : IRequestHandler<DeleteInstructorCommand, GeneralResponse<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteInstructorHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GeneralResponse<bool>> Handle(DeleteInstructorCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var instructor = await _unitOfWork.InstructorRepository.GetByIdAsync(request.Id);

                if (instructor == null)
                    return GeneralResponse<bool>.FailResponse("Instructor not found.", false);

                //Check if instructor has any courses
                if (instructor.Courses != null && instructor.Courses.Any())
                {
                    return GeneralResponse<bool>.FailResponse(
                        "Instructor cannot be deleted because they are assigned to one or more courses.",
                        false
                    );
                }

                // delete
                await _unitOfWork.InstructorRepository.DeleteAsync(i => i.Id == request.Id);
                await _unitOfWork.SaveAsync();

                return GeneralResponse<bool>.SuccessResponse("Instructor deleted successfully.", true);
            }
            catch (Exception)
            {
                return GeneralResponse<bool>.FailResponse("Failed to delete instructor due to an error.", false);
            }
        }
    }
}
