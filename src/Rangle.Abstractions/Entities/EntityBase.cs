using System.ComponentModel.DataAnnotations;

namespace Rangle.Abstractions.Entities
{
    public abstract class EntityBase<TKey>
    {
        [Key, Required]
        public TKey Id { get; set; }
    }
}
