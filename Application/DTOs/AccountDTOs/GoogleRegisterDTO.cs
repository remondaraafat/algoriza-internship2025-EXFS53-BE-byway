using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static APICoursePlatform.Enums.Enums;

namespace Application.DTOs.AccountDTOs
{
    public class GoogleRegisterDTO
    {
        public string Email { get; set; }
        public string FullName { get; set; }
        public string ProviderKey { get; set; } 
        public string UserRole { get; set; }
    }
}
