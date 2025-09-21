using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static APICoursePlatform.Enums.Enums;

namespace Application.DTOs.InstructorDTOs
{
    public class UpdateInstructorDto
    {
        [Required(ErrorMessage = "Instructor Id is required.")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Instructor name is required.")]
        [MaxLength(100, ErrorMessage = "Instructor name cannot exceed 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Instructor bio is required.")]
        [MaxLength(1000, ErrorMessage = "Instructor bio cannot exceed 1000 characters.")]
        public string Bio { get; set; }

        [Required(ErrorMessage = "Job title is required.")]
        public JobTitle JobTitle { get; set; }

        [Required(ErrorMessage = "Rating is required.")]
        [Range(1, 5, ErrorMessage = "Rating must be between {1} and {2}.")]
        public int Rating { get; set; }
        [Required(ErrorMessage = "Image file is required")]
        public IFormFile ImageFile { get; set; }
    }

}
