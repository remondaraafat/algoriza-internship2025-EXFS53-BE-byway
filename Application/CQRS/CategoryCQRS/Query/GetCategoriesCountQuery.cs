using APICoursePlatform.Helpers;
using APICoursePlatform.UnitOfWorkContract;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.CategoryCQRS.Query
{
    public class GetCategoriesCountQuery : IRequest<GeneralResponse<int>>
    {
    }

    public class GetCategoriesCountQueryHandler : IRequestHandler<GetCategoriesCountQuery, GeneralResponse<int>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetCategoriesCountQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GeneralResponse<int>> Handle(GetCategoriesCountQuery request, CancellationToken cancellationToken)
        {
            var count = await _unitOfWork.categoryRepository.CountAsync();

            return GeneralResponse<int>.SuccessResponse("Success Response", count);
        }
    }
}
