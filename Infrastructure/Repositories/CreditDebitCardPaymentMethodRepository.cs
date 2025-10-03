using Infrastructure.Persistence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    internal class CreditDebitCardPaymentMethodRepository : GenericRepository<CreditDebitCardPaymentMethod>, ICreditDebitCardPaymentMethodRepository
    {
        public CreditDebitCardPaymentMethodRepository(CoursePlatformContext context) : base(context)
        {
        }
    }
}
