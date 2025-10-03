using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.AccountDTOs
{
    public class MicrosoftLoginDTO
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Provider key is required.")]
        public string ProviderKey { get; set; }
    }
}
