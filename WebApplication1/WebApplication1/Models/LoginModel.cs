using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class LoginModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The string must be  {1} long", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; } 

    }
}
