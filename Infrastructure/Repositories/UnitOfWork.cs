using APICoursePlatform.Repository;
using APICoursePlatform.RepositoryContract;
using Infrastructure.Persistence.Data;

namespace APICoursePlatform.UnitOfWorkContract
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly CoursePlatformContext _context;

        
        //private INotificationRepository _notificationRepository;
        //private IPaymentRepository _paymentRepository;
        

        public UnitOfWork(CoursePlatformContext context)
        {
            _context = context;
        }
        









        //public INotificationRepository NotificationRepository
        //{
        //    get
        //    {
        //        if (_notificationRepository == null)
        //            _notificationRepository = new NotificationRepository(_context);
        //        return _notificationRepository;
        //    }
        //}

        //public IPaymentRepository PaymentRepository
        //{
        //    get
        //    {
        //        if (_paymentRepository == null)
        //            _paymentRepository = new PaymentRepository(_context);
        //        return _paymentRepository;
        //    }
        //}









        //public IPasswordResetCodeRepository PasswordResetCodeRepository
        //{
        //    get
        //    {
        //        if (_passwordResetCodeRepository == null)
        //            _passwordResetCodeRepository = new PasswordResetCodeRepository(_context);
        //        return _passwordResetCodeRepository;
        //    }
        //}

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
