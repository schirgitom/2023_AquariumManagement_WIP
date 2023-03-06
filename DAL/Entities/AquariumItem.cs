using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public abstract class AquariumItem : Entity
    {
        public AquariumItem() { }
        
        public String Aquarium { get; set; }

        public String Name { get; set; }

        public String Species { get; set; }

        public DateTime Inserted { get; set; } = DateTime.Now;

        public int Amount { get; set; }

        public String Description { get; set; }
    }
}
