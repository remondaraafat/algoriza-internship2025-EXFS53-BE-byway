using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static APICoursePlatform.Enums.Enums;

namespace Application.DTOs.CourseDTOs
{
    public class GetCourseByIdDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsBought { get; set; }
        public bool IsInCart { get; set; }
        public string Description { get; set; }
        public DateTime ReleaseDate { get; set; }

        public string Certificate { get; set; }

     
        public decimal Price { get; set; }

      
        public CourseLevel Level { get; set; }

     
        public int Rating { get; set; }
        public int NumberOfLectures { get; set; }

        public double TotalHours { get; set; }

       
        public string ImageUrl{ get; set; }

        [Required(ErrorMessage = "Category Id is required")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Instructor Id is required")]
        public int InstructorId { get; set; }
    }
}
