using APICoursePlatform.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static APICoursePlatform.Enums.Enums;

namespace Domain.Models
{
    public class Course:BaseEntity
    {

        
        [Required, MaxLength(200)]
        public string Title { get; set; }
        [Required, MaxLength(1000)]
        public string Description { get; set; }
        [Required, MaxLength(1000)]
        public string Certificate { get; set; }
        

        [Required,Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Required]
        public CourseLevel Level { get; set; }

       
        [Required,Range(1, 5, ErrorMessage = "Rating must be between {1} and {2}")]
        public int Rating { get; set; } 

        [Required]
        public double TotalHours { get; set; }
        
        [Required,MaxLength(250)]
        public string ImageUrl { get; set; } 
        // Foreign keys
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public int InstructorId { get; set; }
        public Instructor Instructor { get; set; }

        // Navigation
        public ICollection<Lecture> Lectures { get; set; } = new List<Lecture>();
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
        public ICollection<PaymentCourse> PaymentCourses { get; set; } = new List<PaymentCourse>();

    }
}
