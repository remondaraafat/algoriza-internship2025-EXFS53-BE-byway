using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static APICoursePlatform.Enums.Enums;

namespace Application.DTOs.CreditDebitCardPaymentMethodDTOs
{
    public class CreateCreditDebitCardPaymentMethodDto
    {
        
        [Required(ErrorMessage = "Cardholder name is required.")]
        [MaxLength(50, ErrorMessage = "Cardholder name cannot exceed 50 characters.")]
        public string CardName { get; set; }

        [Required(ErrorMessage = "Card number is required.")]
        [RegularExpression(@"^\d{13,19}$", ErrorMessage = "Card number must be between 13 and 19 digits.")]
        [MaxLength(20, ErrorMessage = "Card number cannot exceed 20 digits.")]
        public string CardNumber { get; set; }

        [Required(ErrorMessage = "Expiry date is required.")]
        [MaxLength(7, ErrorMessage = "Expiry cannot exceed 7 characters.")]
        [RegularExpression(@"^(0[1-9]|1[0-2])/(?:[0-9]{2}|[0-9]{4})$",
            ErrorMessage = "Expiry must be in MM/YY or MM/YYYY format.")]
        public string Expiry { get; set; }

        [Required(ErrorMessage = "CVV is required.")]
        [RegularExpression(@"^\d{3,4}$", ErrorMessage = "CVV must be 3 or 4 digits.")]
        [MaxLength(4, ErrorMessage = "CVV cannot exceed 4 digits.")]
        public string CVV { get; set; }
        
    }
}
