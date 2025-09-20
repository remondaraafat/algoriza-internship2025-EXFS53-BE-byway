using Domain.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static APICoursePlatform.Enums.Enums;

namespace APICoursePlatform.Models
{
    public class ApplicationUser:IdentityUser
    {
        [Required]
        [MaxLength(100)]
        public string FullName { get; set; }

        [MaxLength(250)]
        public string ProfileImageUrl { get; set; } ="/images/default-user.png";//default image

        // Navigation
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();

    }
}
