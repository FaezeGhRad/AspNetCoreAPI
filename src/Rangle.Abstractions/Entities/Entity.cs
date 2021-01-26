using System;

namespace Rangle.Abstractions.Entities
{
    public class Entity : EntityBase<Guid>
    {
        public string JsonObject { get; set; }
        public int CreatorId { get; set; }
        public UserEntity Creator { get; set; }
    }
}
