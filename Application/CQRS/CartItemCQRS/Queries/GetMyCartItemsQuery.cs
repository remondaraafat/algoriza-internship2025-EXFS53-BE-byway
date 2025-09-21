global using Microsoft.EntityFrameworkCore;
using APICoursePlatform.UnitOfWorkContract;
using Application.DTOs.CartItemDTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.CartItemCQRS.Queries
{
    public class GetMyCartItemsQuery : IRequest<List<GetCartItemDto>>
    {
        public string UserId { get; set; }
    }

    public class GetMyCartItemsHandler : IRequestHandler<GetMyCartItemsQuery, List<GetCartItemDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetMyCartItemsHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<GetCartItemDto>> Handle(GetMyCartItemsQuery request, CancellationToken cancellationToken)
        {
            var cartItems = await _unitOfWork.CartItemRepository.GetWithFilterAsync(c => c.UserId == request.UserId)
                .Select(c => new GetCartItemDto
                {
                    CourseId = c.CourseId,
                    UserId = c.UserId,
                    CourseTitle = c.Course.Title,
                    TotalHours = c.Course.TotalHours,
                    NumOfLectures = c.Course.Lectures.Count,
                    Price = c.Course.Price,
                    InstructorName = c.Course.Instructor.Name,
                    Level = c.Course.Level.ToString(),
                    Rating = c.Course.Rating
                })
                .ToListAsync(cancellationToken);

            return cartItems;
        }
    }
}

