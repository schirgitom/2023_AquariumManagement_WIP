using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class DBSettings
    {
        public String DatabaseName { get; set; }

        public String Server { get; set; }

        public int Port { get; set; }

        public String Username { get; set; }

        public String Password { get; set; }
    }
}
