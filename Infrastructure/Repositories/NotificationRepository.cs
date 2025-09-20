//using Infrastructure.Persistence.Data;
//using APICoursePlatform.Models;
//using APICoursePlatform.RepositoryContract;
//using Microsoft.EntityFrameworkCore;

//namespace APICoursePlatform.Repository
//{
//    public class NotificationRepository:GenericRepository<Notification>,INotificationRepository
//    {
//        private readonly CoursePlatformContext context;

//        public NotificationRepository(CoursePlatformContext context) : base(context)
//        {
//            this.context = context;
//        }

//        public async Task<IEnumerable<Notification>> GetUnreadNotification(string userId)
//        {
//            return await context.Notifications.Where(n=>n.RecipientId == userId && !n.IsRead).ToListAsync();
//        }
//    }
//}
