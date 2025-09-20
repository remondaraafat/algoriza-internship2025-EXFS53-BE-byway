using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.AccountDTOs
{
    public class MicrosoftRegisterDTO
    {
        public string Email { get; set; }
        public string FullName { get; set; }
        public string ProviderKey { get; set; } 
        public string UserRole { get; set; }
    }
}
