using APICoursePlatform.Helpers;
using APICoursePlatform.UnitOfWorkContract;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.LectureCQRS.Command
{
    public class DeleteLectureCommand : IRequest<GeneralResponse<string>>
    {
        public int Id { get; set; }

        public DeleteLectureCommand(int id)
        {
            Id = id;
        }
    }

    public class DeleteLectureCommandHandler : IRequestHandler<DeleteLectureCommand, GeneralResponse<string>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteLectureCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GeneralResponse<string>> Handle(DeleteLectureCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var lecture = await _unitOfWork.lectureRepository.GetByIdAsync(request.Id);

                if (lecture == null)
                    return GeneralResponse<string>.FailResponse("Lecture not found");

                await _unitOfWork.lectureRepository.DeleteAsync(l => l.Id == request.Id);
                await _unitOfWork.SaveAsync();

                return GeneralResponse<string>.SuccessResponse("Lecture deleted successfully");
            }
            catch (Exception ex)
            {
                return GeneralResponse<string>.FailResponse($"Error deleting lecture: {ex.Message}");
            }
        }
    }
}
