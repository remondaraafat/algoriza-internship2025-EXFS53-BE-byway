using APICoursePlatform.Repository;
using APICoursePlatform.RepositoryContract;
using Application.Interfaces;
using Infrastructure.Persistence.Data;
using Infrastructure.Repositories;

namespace APICoursePlatform.UnitOfWorkContract
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly CoursePlatformContext _context;


        //private INotificationRepository _notificationRepository;
        private IPaymentRepository _paymentRepository;
        private IInstructorRepository _instructorRepository;
        private ICartItemRepository _cartItemRepository;
        private ICategoryRepository _categoryRepository;
        private ICourseRepository _courseRepository;
        private IPaymentCourseRepository _paymentCourseRepository;
        private IPayPalPaymentMethodRepository _payPalMethodRepository;
        private ICreditDebitCardPaymentMethodRepository _creditDebitCardPaymentMethodRepository;
        private ILectureRepository _lectureRepository;  
        public UnitOfWork(CoursePlatformContext context)
        {
            _context = context;
        }
        public ILectureRepository lectureRepository
        {
            get
            {
                if (_lectureRepository == null)
                    _lectureRepository = new LectureRepository(_context);
                return _lectureRepository;
            }
        }

        public ICreditDebitCardPaymentMethodRepository creditDebitCardPaymentMethodRepository
        {
            get
            {
                if (_creditDebitCardPaymentMethodRepository == null)
                    _creditDebitCardPaymentMethodRepository = new CreditDebitCardPaymentMethodRepository(_context);
                return _creditDebitCardPaymentMethodRepository;


            }
        }
        public IPayPalPaymentMethodRepository payPalMethodRepository
        {
            get
            {
                if (_payPalMethodRepository == null)
                    _payPalMethodRepository = new PayPalMethodRepository(_context);
                return _payPalMethodRepository;
            }
        }
        public IPaymentRepository paymentRepository {
            get
            {

                if (_paymentRepository == null)
                    _paymentRepository = new PaymentRepository(_context);
                return _paymentRepository;
            } 
        }

        public ICourseRepository courseRepository
        {
            get
            {
                if (_courseRepository == null)
                    _courseRepository = new CourseRepository(_context);
                return _courseRepository;
            }
        }
        public ICategoryRepository categoryRepository
        {
            get
            {
                if (_categoryRepository == null)
                    _categoryRepository = new CategoryRepository(_context);
                return _categoryRepository;
            }
        }






        public IInstructorRepository InstructorRepository
        {
            get
            {
                if (_instructorRepository == null)
                    _instructorRepository = new InstructorRepository(_context);
                return _instructorRepository;
            }
        }
        public ICartItemRepository CartItemRepository
        {
            get
            {
                if (_cartItemRepository == null)
                    _cartItemRepository = new CartItemRepository(_context);
                return _cartItemRepository;
            }
        }
        public IPaymentCourseRepository paymentCourseRepository
        {
            get
            {
                if (_paymentCourseRepository == null)
                    _paymentCourseRepository = new PaymentCourseRepository(_context);
                return _paymentCourseRepository;
            }
        }
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
        public void Dispose()
        {
            _context.Dispose();
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
    }
}
