using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Coral : AquariumItem
    {
        public CoralType CoralType { get; set; }
    }

    public enum CoralType
    {
        HardCoral,
        SoftCoral
    }
}
