using Infrastructure.Persistence.Data;
using APICoursePlatform.Models;
using APICoursePlatform.RepositoryContract;

namespace APICoursePlatform.Repository
{
    public class PaymentRepository : GenericRepository<Payment>, IPaymentRepository
    {
        public PaymentRepository(CoursePlatformContext context) : base(context)
        {

        }
    }
}
