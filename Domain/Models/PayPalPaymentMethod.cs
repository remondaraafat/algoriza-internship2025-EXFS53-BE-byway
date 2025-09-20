using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class PayPalPaymentMethod:PaymentMethod
    {
        [Required, EmailAddress]
        public string PayPalEmail { get; set; }
    }
}
