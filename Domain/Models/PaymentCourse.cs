using APICoursePlatform.Models;

namespace Domain.Models
{
    public class PaymentCourse : BaseEntity
    {
        public int PaymentId { get; set; }
        public Payment Payment { get; set; }

        public int CourseId { get; set; }
        public Course Course {get; set;}
    }
}
