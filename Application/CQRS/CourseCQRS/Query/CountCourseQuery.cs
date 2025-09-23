using APICoursePlatform.Helpers;
using APICoursePlatform.UnitOfWorkContract;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.CourseCQRS.Query
{
    public class CountCourseQuery : IRequest<GeneralResponse<int>>
    {

    }

    public class CountCourseQueryHandler : IRequestHandler<CountCourseQuery, GeneralResponse<int>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CountCourseQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GeneralResponse<int>> Handle(CountCourseQuery request, CancellationToken cancellationToken)
        {
            
            var count = await _unitOfWork.courseRepository.CountAsync();

            return GeneralResponse<int>.SuccessResponse("Total courses count retrieved.", count);
        }
    }
}
