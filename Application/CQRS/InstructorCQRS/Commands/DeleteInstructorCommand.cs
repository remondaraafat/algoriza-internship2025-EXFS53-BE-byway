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
                await _unitOfWork.InstructorRepository.DeleteAsync(i => i.Id == request.Id);
                await _unitOfWork.SaveAsync();
                return GeneralResponse<bool>.SuccessResponse("Instructor deleted successfully", true);
            }
            catch (Exception ex)
            {
                return GeneralResponse<bool>.FailResponse("Failed to delete instructor", false);
            }
        }
    }
}
