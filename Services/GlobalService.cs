using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class GlobalService
    {
        public UserServices UserService { get; set; }

        public GlobalService(IUnitOfWork uow)
        {
            UnitOfWork uowi = (UnitOfWork)uow;

            UserService = new UserServices(uowi, uowi.Users, this);

        }
    }
}
