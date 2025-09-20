//using APICoursePlatform.DTOs.NotificationDTO;

//using APICoursePlatform.Helpers;
//using MediatR;
//using APICoursePlatform.UnitOfWorkContract;
//using Microsoft.EntityFrameworkCore;

//namespace APICoursePlatform.CQRS.Notification.Queries
//{
//    public class GetUserNotificationsQuery : IRequest<GeneralResponse<GetDataResult<List<NotificationDTO>>>>
//    {
//        public string UserId { get; }
//        public int PageNumber { get; set; }
//        public int PageSize { get; set; }

//        public GetUserNotificationsQuery(string userId, int pageNumber, int pageSize)
//        {
//            UserId = userId;
//            PageNumber = pageNumber;
//            PageSize = pageSize;
//        }
//    }

//    public class GetUserNotificationsQueryHandler : IRequestHandler<GetUserNotificationsQuery, GeneralResponse<GetDataResult<List<NotificationDTO>>>>
//    {
//        private readonly IUnitOfWork _unitOfWork;

//        public GetUserNotificationsQueryHandler(IUnitOfWork unitOfWork)
//        {
//            _unitOfWork = unitOfWork;
//        }

   
//        public async Task<GeneralResponse<GetDataResult<List<NotificationDTO>>>> Handle(GetUserNotificationsQuery request, CancellationToken cancellationToken)
//        {

//            var notifications = await _unitOfWork.NotificationRepository
//                .GetWithFilterAsync(n => n.RecipientId == request.UserId && !n.IsDeleted)
//                .OrderByDescending(o => o.CreatedAt)
//                .Skip((request.PageNumber - 1) * request.PageSize)
//                .Take(request.PageSize)
//                .Select(n => new NotificationDTO
//                {
//                    Title = n.Title,
//                    Message = n.Message,
//                    CreatedAt = n.CreatedAt,
//                    NotificationType = n.Type
//                }).ToListAsync(cancellationToken);

//            var dataResult = new GetDataResult<List<NotificationDTO>>
//            {
//                Data = notifications,
//                TotalCount = await _unitOfWork.NotificationRepository.CountAsync(n => n.RecipientId == request.UserId && !n.IsDeleted),
//                PageNumber = request.PageNumber,
//                PageSize = request.PageSize
//            };
//            return GeneralResponse<GetDataResult<List<NotificationDTO>>>.SuccessResponse("Notifications retrieved successfully",dataResult);
//        }


//    }
//}
