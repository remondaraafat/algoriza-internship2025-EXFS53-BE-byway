using APICoursePlatform.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static APICoursePlatform.Enums.Enums;

namespace Domain.Models
{
    public class Instructor:BaseEntity
    {
        [Required, MaxLength(100)]
        public string Name { get; set; }
        [Required, MaxLength(1000)]
        public string Bio { get; set; }
        [Required]
        public JobTitle jobTitle { get; set; }
        [Required, Range(1, 5, ErrorMessage = "Rating must be between {1} and {2}")]
        public int Rating { get; set; }
        [Required, MaxLength(250)]
        public string ImageUrl { get; set; }



        public ICollection<Course> Courses { get; set; } = new List<Course>();
    }
}
