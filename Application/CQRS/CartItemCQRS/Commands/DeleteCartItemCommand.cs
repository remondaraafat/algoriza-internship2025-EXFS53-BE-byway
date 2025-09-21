using APICoursePlatform.Helpers;
using APICoursePlatform.UnitOfWorkContract;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.CartItemCQRS.Commands
{
    // Command
    public class DeleteCartItemCommand : IRequest<GeneralResponse<bool>>
    {
        public int CartItemId { get; set; }
        public string UserId { get; set; }

      
    }

    // Handler
    public class DeleteCartItemHandler : IRequestHandler<DeleteCartItemCommand, GeneralResponse<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCartItemHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GeneralResponse<bool>> Handle(DeleteCartItemCommand request, CancellationToken cancellationToken)
        {
            // ensure item exists and belongs to user
            var existing = await _unitOfWork.CartItemRepository
                .GetWithFilterAsync(ci => ci.Id == request.CartItemId && ci.UserId == request.UserId)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);

            if (existing == null)
                return GeneralResponse<bool>.FailResponse("Cart item not found");

            await _unitOfWork.CartItemRepository.DeleteAsync(ci => ci.Id == request.CartItemId && ci.UserId == request.UserId);
            await _unitOfWork.SaveAsync();

            return GeneralResponse<bool>.SuccessResponse("Cart item deleted successfully", true);
        }
    }
}
