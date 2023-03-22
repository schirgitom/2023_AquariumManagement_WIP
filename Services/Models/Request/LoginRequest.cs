using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models.Request
{
    public class LoginRequest
    {
        public String Email { get; set; }

        public String Password { get; set; }
    }
}
