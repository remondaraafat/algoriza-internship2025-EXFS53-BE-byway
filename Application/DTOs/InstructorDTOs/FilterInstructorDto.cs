using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static APICoursePlatform.Enums.Enums;

namespace Application.DTOs.InstructorDTOs
{
    public class FilterInstructorDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public JobTitle JobTitle { get; set; }
        public int Rating { get; set; }
        public string ImageUrl { get; set; }
    }
}
