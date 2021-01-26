using System;

namespace Rangle.API.Models
{
    public class EntityModel
    {
        public Guid Id { get; set; }

        public object JsonObject { get; set; }
    }
}
