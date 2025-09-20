using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class CreditDebitCardPaymentMethod:PaymentMethod
    {
        [Required, MaxLength(50)]
        public string CardName { get; set; }
        [Required, MaxLength(20)]
        public string CardNumber { get; set; }

        [Required]
        [MaxLength(7)]  
        [RegularExpression(@"^(0[1-9]|1[0-2])/(?:[0-9]{2}|[0-9]{4})$",
        ErrorMessage = "Expiry must be in MM/YY or MM/YYYY format.")]
        public string Expiry { get; set; }

        [Required, MaxLength(4)]
        public string CVV { get; set; }

       
    }
}
