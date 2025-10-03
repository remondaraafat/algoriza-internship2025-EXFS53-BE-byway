using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.AccountDTOs
{
    public class MicrosoftRegisterDTO
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Full name is required.")]
        [MaxLength(100, ErrorMessage = "Full name cannot exceed 100 characters.")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Provider key is required.")]
        public string ProviderKey { get; set; }

        [Required(ErrorMessage = "User role is required.")]
        public string UserRole { get; set; }
    }
}
