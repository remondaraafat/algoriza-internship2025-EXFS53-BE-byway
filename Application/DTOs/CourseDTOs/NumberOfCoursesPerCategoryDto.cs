using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.CourseDTOs
{
    public class NumberOfCoursesPerCategoryDTO
    {
        public int CategoryID { get; set; } 
        public string? CategoryName { get; set; }
        public int NumberOfCourses { get; set; }
    }
}
