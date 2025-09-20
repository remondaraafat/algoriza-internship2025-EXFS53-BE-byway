using APICoursePlatform.RepositoryContract;

namespace APICoursePlatform.UnitOfWorkContract
{
    public interface IUnitOfWork : IDisposable
    {
        
      
        //INotificationRepository NotificationRepository { get; }
        //IPaymentRepository PaymentRepository { get; }
       
        //IPasswordResetCodeRepository PasswordResetCodeRepository { get; }



        Task SaveAsync();
    }
}
