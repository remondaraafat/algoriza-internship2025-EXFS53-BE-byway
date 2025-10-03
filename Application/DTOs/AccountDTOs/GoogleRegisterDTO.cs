using System.ComponentModel.DataAnnotations;
using static APICoursePlatform.Enums.Enums;

namespace Application.DTOs.AccountDTOs
{
    public class GoogleRegisterDTO
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Full name is required.")]
        [MaxLength(200, ErrorMessage = "Full name cannot exceed 200 characters.")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "ProviderKey is required.")]
        public string ProviderKey { get; set; }

        [Required(ErrorMessage = "UserRole is required.")]
        public string UserRole { get; set; }
    }
}
