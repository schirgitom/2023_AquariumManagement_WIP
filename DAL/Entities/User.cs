using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace DAL.Entities
{
    public class User : Entity
    {
        public string Email { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }

        [BsonIgnore]
        public string FullName
        {
            get
            {
                return Firstname + " " + Lastname;
            }
        }


        [BsonIgnore]
        //  [JsonIgnore]
        //[IgnoreDataMember]
        public string Password { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        public string HashedPassword { get; set; }

        public Boolean Active { get; set; }
    }


}
