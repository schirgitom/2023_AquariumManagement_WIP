using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace DAL.Entities
{
    public class Aquarium : Entity
    {
        public String Name { get; set; }

        //[JsonConverter(typeof(WaterType))]
        [BsonRepresentation(BsonType.String)]
        [EnumDataType(typeof(WaterType))]
        public WaterType WaterType { get; set; }

        public Double Depth { get; set; }

        public Double Height { get; set; }

        public Double Length { get; set; }


        public Double Liters { get; set; }


    }
}
