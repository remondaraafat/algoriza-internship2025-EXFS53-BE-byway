using APICoursePlatform.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static APICoursePlatform.Enums.Enums;

namespace Domain.Models
{
    public class PaymentMethod:BaseEntity
    {
        [Required, MaxLength(50)]
        public PaymentMethodType MethodType { get; set; }
    }
}
