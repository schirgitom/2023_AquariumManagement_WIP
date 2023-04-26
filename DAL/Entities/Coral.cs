using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace DAL.Entities
{
    public class Coral : AquariumItem
    {
        // [JsonConverter(typeof(CoralType))]
        [BsonRepresentation(BsonType.String)]
        [EnumDataType(typeof(CoralType))]
        public CoralType CoralType { get; set; }



    }
}
