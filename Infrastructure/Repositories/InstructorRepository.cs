
using APICoursePlatform.Repository;
using APICoursePlatform.RepositoryContract;
using Application.Interfaces;
using Domain.Models;
using Infrastructure.Persistence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    internal class InstructorRepository : GenericRepository<Instructor>, IInstructorRepository
    {
        public InstructorRepository(CoursePlatformContext context) : base(context)
        {
        }
    }
}
