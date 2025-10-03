using APICoursePlatform.Helpers;
using APICoursePlatform.UnitOfWorkContract;
using Application.DTOs.CartItemDTOs;
using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.CartItemCQRS.Commands
{
    public class CreateCartItemCommand : IRequest<GeneralResponse<GetCartItemDto>>
    {
        public CreateCartItemDto Dto { get; set; }

    }

    public class CreateCartItemHandler : IRequestHandler<CreateCartItemCommand, GeneralResponse<GetCartItemDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateCartItemHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GeneralResponse<GetCartItemDto>> Handle(CreateCartItemCommand request, CancellationToken cancellationToken)
        {
            var exists = await _unitOfWork.CartItemRepository
                .ExistsAsync(ci => ci.UserId == request.Dto.UserId && ci.CourseId == request.Dto.CourseId);

            if (exists)
                return GeneralResponse<GetCartItemDto>.FailResponse("Course already exists in cart");

            bool isBought = await _unitOfWork.paymentCourseRepository
                .ExistsAsync(pc => pc.CourseId == request.Dto.CourseId && pc.Payment.UserId == request.Dto.UserId);

            if (isBought)
                return GeneralResponse<GetCartItemDto>.FailResponse("Course already Bought");

            var cartItem = new CartItem
            {
                UserId = request.Dto.UserId,
                CourseId = request.Dto.CourseId
            };

            await _unitOfWork.CartItemRepository.AddAsync(cartItem);
            await _unitOfWork.SaveAsync();

            return GeneralResponse<GetCartItemDto>.SuccessResponse("Cart item added successfully");
        }
    }
}
