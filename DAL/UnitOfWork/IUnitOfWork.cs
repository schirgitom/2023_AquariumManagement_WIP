using DAL.Repository.Impl;

namespace DAL.UnitOfWork
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
