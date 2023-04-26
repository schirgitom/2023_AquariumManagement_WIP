using DAL.Repository.Impl;

namespace DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        public DBContext Context { get; private set; } = null;
        public UnitOfWork()
        {
            DBContext context = new DBContext();
            Context = context;
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

        public IPictureRepository Pictures
        {
            get
            {
                return new PictureRepository(Context);
            }
        }

    }
}
