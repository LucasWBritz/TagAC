using System.ComponentModel.DataAnnotations;

namespace TagAC.Apis.Identity.Models
{
    public class RegisterUserModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Both passwords must match.")]
        public string PasswordConfirmation { get; set; }
    }
}
