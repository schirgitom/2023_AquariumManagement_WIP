using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Entity : IEntity
    {
        public string ID { get; set; }

        public string GenerateID()
        {
            throw new NotImplementedException();
        }
    }
}
