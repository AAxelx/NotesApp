using MongoDB.Bson;
using Newtonsoft.Json;

namespace NotesApp.Api.Helpers
{
    public class ObjectIdConverter : JsonConverter<ObjectId>
    {
        public override ObjectId ReadJson(JsonReader reader, Type objectType, ObjectId existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.String)
            {
                string value = reader.Value.ToString();
                if (ObjectId.TryParse(value, out ObjectId objectId))
                {
                    return objectId;
                }
            }

            return existingValue;
        }

        public override void WriteJson(JsonWriter writer, ObjectId value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }
    }
}

