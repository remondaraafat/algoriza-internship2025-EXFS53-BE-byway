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
    public class CreateCourseDto
    {

        [Required(ErrorMessage = "Course title is required")]
        [MaxLength(200, ErrorMessage = "Course title cannot exceed {1} characters")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Course description is required")]
        [MaxLength(1000, ErrorMessage = "Description cannot exceed {1} characters")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Certificate field is required")]
        [MaxLength(1000, ErrorMessage = "Certificate description cannot exceed {1} characters")]
        public string Certificate { get; set; }

        [Required(ErrorMessage = "Course price is required")]
        [Column(TypeName = "decimal(18,2)")]
        [Range(1, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Course level is required")]
        public CourseLevel Level { get; set; }

        [Required(ErrorMessage = "Rating is required")]
        [Range(1, 5, ErrorMessage = "Rating must be between {1} and {2}")]
        public int Rating { get; set; }

        [Required(ErrorMessage = "Total hours are required")]
        [Range(0.5, double.MaxValue, ErrorMessage = "Total hours must be greater than 0")]
        public double TotalHours { get; set; }

        [Required(ErrorMessage = "Image file is required")]

        public IFormFile ImageFile { get; set; }

        [Required(ErrorMessage = "Category Id is required")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Instructor Id is required")]
        public int InstructorId { get; set; }

    }
}
