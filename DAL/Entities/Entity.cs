using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Entities
{
    public abstract class Entity : IEntity
    {
        /// <summary>
        /// This property is auto managed. A new ID will be assigned for new entities upon saving.
        /// </summary>
        [BsonId]
        public string ID { get; set; }

        /// <summary>
        /// Override this method in order to control the generation of IDs for new entities.
        /// </summary>
        public virtual string GenerateNewID()
            => ObjectId.GenerateNewId().ToString();
    }
}
