using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DAL.Entities
{
    public class UserAquarium : Entity
    {

        public String UserID { get; set; }

        public String AquariumID { get; set; }

        [BsonRepresentation(BsonType.String)]
        [JsonConverter(typeof(StringEnumConverter))]
        public UserRole Role { get; set; }

    }
}
