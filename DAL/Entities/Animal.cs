using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Animal : AquariumItem
    {
        public DateTime DeathDate { get; set; } = DateTime.MinValue;

        public Boolean IsAlive { get; set; }
    }
}
