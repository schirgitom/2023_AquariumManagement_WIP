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
    public interface IUnitOfWork
    {
        DBContext Context { get; }

        IAquariumRepository Aquariums { get; }

        IAquariumItemRepository AquariumItems { get; }

        IUserRepository Users { get; }

        IUserAquariumRepository UsersAquarium { get; }
    }
}
