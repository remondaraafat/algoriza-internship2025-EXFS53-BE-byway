using APICoursePlatform.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Lecture:BaseEntity
    {
        

        [Required, MaxLength(200)]
        public string Title { get; set; }
        [Required]
        public int Order { get; set; }
       
        [Required,Range(1, 120, ErrorMessage = "Duration must be between 1 and 120 minutes.")]
        public int DurationMinutes { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }
    }
}
