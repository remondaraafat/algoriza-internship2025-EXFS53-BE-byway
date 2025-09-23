global using Microsoft.EntityFrameworkCore;
using APICoursePlatform.Helpers;
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
    public class GetMyCartItemsQuery : IRequest<GeneralResponse<PagedResult<GetCartItemDto>>>
    {
        public string UserId { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }


    public class GetMyCartItemsHandler : IRequestHandler<GetMyCartItemsQuery, GeneralResponse<PagedResult<GetCartItemDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetMyCartItemsHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GeneralResponse<PagedResult<GetCartItemDto>>> Handle(GetMyCartItemsQuery request, CancellationToken cancellationToken)
        {
            try {
                var query = _unitOfWork.CartItemRepository
               .GetWithFilterAsync(c => c.UserId == request.UserId, request.PageNumber, request.PageSize);

                var items = await query.Select(c => new GetCartItemDto
                {
                    Id = c.Id,
                    CourseId = c.CourseId,
                    UserId = c.UserId,
                    CourseTitle = c.Course.Title,
                    TotalHours = c.Course.TotalHours,
                    NumOfLectures = c.Course.Lectures.Count(l => !l.IsDeleted),
                    Price = c.Course.Price,
                    InstructorName = c.Course.Instructor.Name,
                    Level = c.Course.Level.ToString(),
                    Rating = c.Course.Rating
                }).ToListAsync(cancellationToken);

                var totalCount = await _unitOfWork.CartItemRepository.CountAsync(c => c.UserId == request.UserId);

                var pagedResult = new PagedResult<GetCartItemDto>
                {
                    Items = items,
                    TotalCount = totalCount,
                    PageIndex = request.PageNumber,
                    PageSize = request.PageSize
                };

                return GeneralResponse<PagedResult<GetCartItemDto>>.SuccessResponse("Cart items retrieved successfully", pagedResult);

            } catch (Exception ex)
            {
                return GeneralResponse<PagedResult<GetCartItemDto>>.FailResponse($"An error occurred: {ex.Message}");
            }
           
        }
    }

}

