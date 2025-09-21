global using APICoursePlatform.Repository;
global using Application.Interfaces;
global using Domain.Models;
using Infrastructure.Persistence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    internal class CartItemRepository : GenericRepository<CartItem>, ICartItemRepository
    {
        public CartItemRepository(CoursePlatformContext context) : base(context)
        {
        }
    }
}
