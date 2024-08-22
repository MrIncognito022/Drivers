using System.ComponentModel.DataAnnotations;

namespace Drivers.API.Models.DtOs
{
    public class UserLoginRequestDto
    {
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; }= string.Empty;
    }
}
