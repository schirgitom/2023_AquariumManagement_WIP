using DAL.Entities;
using DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        public DBContext Context { get; private set; } = null;

        public IRepository<Aquarium> Aquarium => new Repository<Aquarium>(Context);



        public UnitOfWork() {
            
            Context = new DBContext();
        
        }
    }
}
