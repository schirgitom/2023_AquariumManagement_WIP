using DAL.Entities;

namespace DAL.Repository.Impl
{
    public class UserAquariumRepository : Repository<UserAquarium>, IUserAquariumRepository
    {
        public UserAquariumRepository(DBContext context) : base(context)
        {

        }
    }
}
