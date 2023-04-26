using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace DAL.Entities
{


    [BsonDiscriminator(RootClass = true)]
    [BsonKnownTypes(typeof(Animal), typeof(Coral))]
    // [JsonConverter(typeof(JsonInheritanceConverter), "discriminator")]
    [KnownType(typeof(Animal))]
    [KnownType(typeof(Coral))]
    public class AquariumItem : Entity
    {
        [JsonRequired]
        public String Aquarium { get; set; }

        [JsonRequired]
        public String Name { get; set; }

        [JsonRequired]
        public String Species { get; set; }

        public DateTime Inserted { get; set; } = DateTime.Now;

        [JsonRequired]
        public int Amount { get; set; }

        public String Description { get; set; }
    }
}
