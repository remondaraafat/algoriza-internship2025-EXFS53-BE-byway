using Infrastructure.Persistence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    internal class LectureRepository : GenericRepository<Lecture>, ILectureRepository
    {
        public LectureRepository(CoursePlatformContext context) : base(context)
        {
        }
    }
}
