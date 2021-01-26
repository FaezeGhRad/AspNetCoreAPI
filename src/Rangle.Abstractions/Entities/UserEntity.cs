using System.ComponentModel.DataAnnotations;

namespace Rangle.Abstractions.Entities
{
    public class UserEntity : EntityBase<int>
    {
        [Required, MinLength(6), MaxLength(256)]
        public string Username { get; set; }

        [Required, MinLength(6), MaxLength(256)]
        public string Password { get; set; }
    }
}
