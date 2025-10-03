using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static APICoursePlatform.Enums.Enums;

namespace Application.DTOs.PayPalPaymentMethodDTOs
{
    public class CreatePayPalPaymentMethodDto
    {
        
        [Required(ErrorMessage = "PayPal email is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid PayPal email address.")]
        [MaxLength(150, ErrorMessage = "PayPal email cannot exceed 150 characters.")]
        public string PayPalEmail { get; set; }
       
    }
}
