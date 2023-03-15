using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class User : Entity
    {
        public String Email { get; set; }
        
        public String Firstname { get; set; }

        public String Lastname { get; set; }

        public String Fullname
        {
            get
            {
                return Firstname + " " + Lastname;
            }
        }

        [JsonIgnore]
        [BsonIgnore]
        public String Password { get; set; }

        [JsonIgnore]
        public String HashedPassword { get; set; }

        public Boolean IsActive { get; set; }
    }
}
