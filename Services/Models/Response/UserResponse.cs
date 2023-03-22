using DAL.Entities;
using Services.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models.Response
{
    public class UserResponse
    {
        public User User { get; set; }

        public AuthenticationInformation AuthenticationInformation { get; set; }
    }
}
