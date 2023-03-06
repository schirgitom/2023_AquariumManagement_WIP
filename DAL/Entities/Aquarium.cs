using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Aquarium : Entity
    {
        public String Name { get; set; }

        public Double Depth { get; set; }
        public Double Length { get; set; }
        public Double Height { get; set; }

        public WaterType WaterType { get; set; }


        public Double Liters { get; set; }
    }
}
