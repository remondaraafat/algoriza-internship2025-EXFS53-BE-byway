//using MediatR;
//using APICoursePlatform.Helpers;
//using Microsoft.EntityFrameworkCore;
//using APICoursePlatform.UnitOfWorkContract;

//namespace APICoursePlatform.CQRS.Payments.Queries
//{
//    public class GetAllPaymentsAmountQuery : IRequest<GeneralResponse<decimal>> { }

//    public class GetAllPaymentsAmountHandler : IRequestHandler<GetAllPaymentsAmountQuery, GeneralResponse<decimal>>
//    {
//        private readonly IUnitOfWork _unitOfWork;

//        public GetAllPaymentsAmountHandler(IUnitOfWork unitOfWork)
//        {
//            _unitOfWork = unitOfWork;
//        }

//        public async Task<GeneralResponse<decimal>> Handle(GetAllPaymentsAmountQuery request, CancellationToken cancellationToken)
//        {
//            var totalAmount = await _unitOfWork.PaymentRepository
//                .GetAllAsync()
//                .SumAsync(p => p.Amount, cancellationToken);

//            return GeneralResponse<decimal>.SuccessResponse("Total amount retrieved successfully", totalAmount);
//        }
//    }
//}
