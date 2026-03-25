using System.ComponentModel.DataAnnotations;

namespace AdventCalender.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Придумайте себе никнейм")]
        [Display(Name = "Никнейм")]
        public string Nickname { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)] 
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required]
        public string Role { get; set; } = "Buyer"; 

        public string? StoreName { get; set; }
    }
}