using DAL.Entities;
using DAL.UnitOfWork;

namespace DAL.Repository.Impl
{
    public class AquariumRepository : Repository<Aquarium>, IAquariumRepository
    {
        public AquariumRepository(DBContext context) : base(context)
        {

        }
        public async Task<Aquarium> GetByName(string name)
        {
            return FindOne(x => x.Name.Equals(name));
        }
    }
}
