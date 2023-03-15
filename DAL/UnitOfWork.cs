using DAL.Entities;
using DAL.Repository;
using DAL.Repository.Impl;
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




        public UnitOfWork() {
            
            Context = new DBContext();
        
        }

        public IAquariumRepository Aquariums
        {
            get
            {
                return new AquariumRepository(Context);
            }
        }

        public IAquariumItemRepository AquariumItems
        {
            get
            {
                return new AquariumItemRepository(Context);
            }
        }

        public IUserRepository Users
        {
            get
            {
                return new UserRepository(Context);
            }
        }

        public IUserAquariumRepository UsersAquarium
        {
            get
            {
                return new UserAquariumRepository(Context);
            }
        }

     
    }
}
