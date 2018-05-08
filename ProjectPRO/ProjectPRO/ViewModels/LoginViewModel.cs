using System.ComponentModel.DataAnnotations;

namespace ProjectPRO.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Must provide e-mail")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Must provide password")]
        public string Password { get; set; }

        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }
    }
}