using System.ComponentModel.DataAnnotations;

namespace Core.Auth.Dtos
{
    public class LoginDto
    {
        [Required]
        [EmailAddress(ErrorMessage = "Invalid format")]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public string? IpAddress { get; set; }

        public string? UserDevice { get; set; }
    }
}
