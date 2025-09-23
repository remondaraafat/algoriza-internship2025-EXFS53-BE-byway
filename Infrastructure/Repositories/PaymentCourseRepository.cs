using Infrastructure.Persistence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    internal class PaymentCourseRepository : GenericRepository<PaymentCourse>, IPaymentCourseRepository
    {
        public PaymentCourseRepository(CoursePlatformContext context) : base(context)
        {
        }
    }
}
