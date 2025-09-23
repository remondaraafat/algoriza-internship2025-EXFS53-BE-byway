using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static APICoursePlatform.Enums.Enums;

namespace Application.DTOs.CourseDTOs
{
    public class FilterCourseDto
    {
        public int Id { get; set; }
        
        public string Title { get; set; }

        public string Description { get; set; }


        public string Certificate { get; set; }


        public decimal Price { get; set; }

        public DateTime ReleaseDate { get; set; }
        public CourseLevel Level { get; set; }


        public int Rating { get; set; }
        public int NumberOfLectures { get; set; }

        public double TotalHours { get; set; }


        public string ImageUrl { get; set; }


        public int CategoryId { get; set; }


        public int InstructorId { get; set; }
    }
}
