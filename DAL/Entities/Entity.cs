using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Entity : IEntity
    {
        [BsonId]
        public string ID { get; set; }

        public string GenerateID()
        {
            return ObjectId.GenerateNewId().ToString();
        }

  
    }
}
