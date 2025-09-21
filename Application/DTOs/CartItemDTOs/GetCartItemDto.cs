using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.CartItemDTOs
{
    public class GetCartItemDto
    {
        public int CourseId { get; set; }
        public string UserId { get; set; }

        public string CourseTitle { get; set; }
        public double TotalHours { get; set; }
        public int NumOfLectures { get; set; }
        public decimal Price { get; set; }
        public string InstructorName { get; set; }
        public string Level { get; set; }
        public double Rating { get; set; }
    }
}
