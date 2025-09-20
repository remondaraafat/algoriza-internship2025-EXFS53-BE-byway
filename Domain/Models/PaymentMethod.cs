using APICoursePlatform.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class PaymentMethod:BaseEntity
    {
        [Required, MaxLength(50)]
        public string MethodName { get; set; }
    }
}
