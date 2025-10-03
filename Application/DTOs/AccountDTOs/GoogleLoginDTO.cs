using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.AccountDTOs
{
    public class GoogleLoginDTO
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "ProviderKey is required.")]

        public string ProviderKey { get; set; }
    }
}
