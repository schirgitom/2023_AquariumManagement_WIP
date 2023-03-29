using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Picture : Entity
    {
        public String Aquarium { get; set; }
        public String Description { get; set; }
        public String ContentType { get; set; }
        public String PictureID { get; set; }
        public DateTime Uploaded { get; set; } = DateTime.Now;
    }
}
