using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Rangle.API.Models;

namespace Rangle.API.Converters
{
    // TODO: use System.Text.Json mechanisim instead of Newtonsoft

    public class EntityJsonConverter : JsonConverter<EntityModel>
    {
        public override bool CanRead => false;

        public override EntityModel ReadJson(JsonReader reader, Type objectType, EntityModel existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, EntityModel value, JsonSerializer serializer)
        {
            // Append internal id to the original object

            if (value == null)
            {
                return;
            }

            JObject jObject = JObject.Parse(value.JsonObject.ToString());
            jObject.Add(new JProperty("_id", value.Id));
            jObject.WriteTo(writer);
        }
    }

}
