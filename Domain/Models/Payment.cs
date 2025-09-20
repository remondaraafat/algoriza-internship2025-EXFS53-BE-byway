using APICoursePlatform.Models;
using Domain.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static APICoursePlatform.Enums.Enums;

public class Payment : BaseEntity
{
    [Required, MaxLength(50)]
    public string Country { get; set; }
    [Required, MaxLength(100)]
    public string State { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount { get; set; }
    public string UserId { get; set; }
    public ApplicationUser User { get; set; }
    // FK to the method used
    public int PaymentMethodId { get; set; }
    public PaymentMethod PaymentMethod { get; set; }

    public ICollection<PaymentCourse> PaymentCourses { get; set; } = new List<PaymentCourse>();

}

