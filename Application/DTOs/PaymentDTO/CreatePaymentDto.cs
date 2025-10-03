using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.PaymentDTO
{
    public class CreatePaymentDto
    {
        [Required(ErrorMessage = "Country is required.")]
        [MaxLength(50, ErrorMessage = "Country cannot exceed 50 characters.")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Country must contain only letters.")]
        public string Country { get; set; }

        [Required(ErrorMessage = "State is required.")]
        [MaxLength(100, ErrorMessage = "State cannot exceed 100 characters.")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "State must contain only letters.")]
        public string State { get; set; }

       
    }
}


