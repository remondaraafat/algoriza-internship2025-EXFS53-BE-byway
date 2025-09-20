//using System.Linq.Expressions;
//using Infrastructure.Persistence.Data;
//using Microsoft.EntityFrameworkCore;
//using APICoursePlatform.Models;
//using APICoursePlatform.RepositoryContract;

//namespace APICoursePlatform.Repository
//{
//    public class PasswordResetCodeRepository:GenericRepository<PasswordResetCode>,IPasswordResetCodeRepository
//    {
//        public PasswordResetCodeRepository(CoursePlatformContext shipConnectContext):base(shipConnectContext)
//        {
//            ShipConnectContext = shipConnectContext;
//        }

//        public CoursePlatformContext ShipConnectContext { get; }

//        public async Task<PasswordResetCode?> GetFirstOrDefaultAsync(Expression<Func<PasswordResetCode, bool>> predicate)
//        {
//            return await _context.Set<PasswordResetCode>().FirstOrDefaultAsync(predicate);
//        }
//    }
//}
