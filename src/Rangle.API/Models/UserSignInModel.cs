using System.ComponentModel.DataAnnotations;

namespace Rangle.API.Models
{
    public class UserSignInModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
