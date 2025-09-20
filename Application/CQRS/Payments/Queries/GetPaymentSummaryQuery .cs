//using APICoursePlatform.DTOs.PaymentDTO;
//using MediatR;
//using APICoursePlatform.Helpers;
//using APICoursePlatform.UnitOfWorkContract;
//using static APICoursePlatform.Enums.Enums;

//namespace APICoursePlatform.CQRS.Payments.Queries
//{
//    public class GetPaymentSummaryQuery : IRequest<GeneralResponse<PaymentSummaryDto>>
//    {
//    }


//    public class GetPaymentSummaryQueryHandler : IRequestHandler<GetPaymentSummaryQuery, GeneralResponse<PaymentSummaryDto>>
//    {
//        private readonly IUnitOfWork _unitOfWork;

//        public GetPaymentSummaryQueryHandler(IUnitOfWork unitOfWork)
//        {
//            _unitOfWork = unitOfWork;
//        }

//        public async Task<GeneralResponse<PaymentSummaryDto>> Handle(GetPaymentSummaryQuery request, CancellationToken cancellationToken)
//        {
//            var payments = _unitOfWork.PaymentRepository.GetAllAsync();

//            var total = payments.Count();
//            var completed = payments.Count(p => p.Status == PaymentStatus.Completed);
//            var failedOrPending = payments.Count(p => p.Status != PaymentStatus.Completed);

//            var dto = new PaymentSummaryDto
//            {
//                TotalPayments = total,
//                CompletedPayments = completed,
//                FailedOrPendingPayments = failedOrPending
//            };

//            return GeneralResponse<PaymentSummaryDto>.SuccessResponse("Payment summary retrieved successfully", dto);
//        }
//    }
//}
