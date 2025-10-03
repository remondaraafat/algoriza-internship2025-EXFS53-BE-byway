using APICoursePlatform.Helpers;
using APICoursePlatform.UnitOfWorkContract;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.PaymentCQRS.Query
{
    // Query
    public class GetTotalPaymentsForCurrentMonthQuery : IRequest<GeneralResponse<decimal>>
    {
    }

    // Handler
    public class GetTotalPaymentsForCurrentMonthQueryHandler
        : IRequestHandler<GetTotalPaymentsForCurrentMonthQuery, GeneralResponse<decimal>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetTotalPaymentsForCurrentMonthQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GeneralResponse<decimal>> Handle(GetTotalPaymentsForCurrentMonthQuery request, CancellationToken cancellationToken)
        {
            try {
                var now = DateTime.UtcNow;
                var startOfMonth = new DateTime(now.Year, now.Month, 1);
                var endOfMonth = startOfMonth.AddMonths(1);

                var totalAmount = await _unitOfWork.paymentRepository
                    .GetWithFilterAsync(p => p.CreatedAt >= startOfMonth && p.CreatedAt < endOfMonth)
                    .SumAsync(p => p.Amount, cancellationToken);

                return GeneralResponse<decimal>.SuccessResponse("Success Response", totalAmount);
            } catch (Exception ex)
            {
                return GeneralResponse<decimal>.FailResponse($"Error: {ex.Message}");
            }
            
        }
    }
}
