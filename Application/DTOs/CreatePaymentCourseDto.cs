using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    internal class CreatePaymentCourseDto
    {
        [Required(ErrorMessage = "Payment Id is required")]
        public int PaymentId { get; set; }
        [Required(ErrorMessage = "Course Id is required")]

        public int CourseId { get; set; }
    }
}
