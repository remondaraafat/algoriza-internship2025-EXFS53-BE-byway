using APICoursePlatform.RepositoryContract;
using Application.Interfaces;

namespace APICoursePlatform.UnitOfWorkContract
{
    public interface IUnitOfWork : IDisposable
    {


        //INotificationRepository NotificationRepository { get; }
        //IPaymentRepository PaymentRepository { get; }

        //IPasswordResetCodeRepository PasswordResetCodeRepository { get; }
        IInstructorRepository InstructorRepository { get; }
        ICartItemRepository CartItemRepository { get; }
        ICategoryRepository categoryRepository { get; }
        ICourseRepository courseRepository { get; }
        Task SaveAsync();
    }
}
