using System.ComponentModel.DataAnnotations;

namespace Rangle.API.Models
{
    public class UserRegisterModel
    {
        [Required, MinLength(6), MaxLength(32)]
        public string Username { get; set; }

        [Required, MinLength(6), MaxLength(32)]
        public string Password { get; set; }
    }
}
