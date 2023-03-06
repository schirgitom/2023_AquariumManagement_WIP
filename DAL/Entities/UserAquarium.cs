using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class UserAquarium : Entity
    {
        public String UserID { get; set; }

        public String AquariumID { get; set; }

        public UserRole Role { get; set; }


    }

    public enum UserRole
    {
        User,
        Admin
    }
}
