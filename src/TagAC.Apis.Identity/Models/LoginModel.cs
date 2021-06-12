using System.ComponentModel.DataAnnotations;

namespace TagAC.Apis.Identity.Models
{
    public class LoginModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(maximumLength: 100, MinimumLength = 6)]
        public string Password { get; set; }
    }
}
