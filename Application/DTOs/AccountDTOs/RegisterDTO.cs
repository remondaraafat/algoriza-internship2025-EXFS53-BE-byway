using System.ComponentModel.DataAnnotations;
using static APICoursePlatform.Enums.Enums;

namespace APICoursePlatform.DTOs.AccountDTOs
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "*")]
        [MaxLength(100, ErrorMessage = "First name cannot exceed 100 characters.")]
        [RegularExpression(@"^\S+$", ErrorMessage = "No spaces are allowed.")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "*")]
        [MaxLength(100, ErrorMessage = "Last cannot exceed 100 characters.")]
        [RegularExpression(@"^\S+$", ErrorMessage = "No spaces are allowed.")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "*")]
        [MaxLength(100, ErrorMessage = "User Name cannot exceed 100 characters.")]
        [RegularExpression(@"^\S+$", ErrorMessage = "No spaces are allowed.")]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "*")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }


        [Required(ErrorMessage = "*")]
        [StringLength(25, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 25 characters.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$",
            ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, and one number.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "*")]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }
        public UserRole UserRole { get; set; }
    }
}

