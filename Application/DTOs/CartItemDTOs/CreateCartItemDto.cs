global using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.CartItemDTOs
{
    public class CreateCartItemDto
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public int CourseId { get; set; }
    }
}
