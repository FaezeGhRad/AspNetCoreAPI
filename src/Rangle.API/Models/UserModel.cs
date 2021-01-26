using System.ComponentModel.DataAnnotations;

namespace Rangle.API.Models
{
    public class UserModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }
    }
}
