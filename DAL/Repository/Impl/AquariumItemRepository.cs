using DAL.Entities;
using DAL.UnitOfWork;
using MongoDB.Driver;

namespace DAL.Repository.Impl
{
    public class AquariumItemRepository : Repository<AquariumItem>, IAquariumItemRepository
    {
        public AquariumItemRepository(DBContext context) : base(context)
        {

        }

        public List<Animal> GetAnimals(String aquarium)
        {
            return Collection.AsQueryable<AquariumItem>().Where(x => x.Aquarium.Equals(aquarium)).OfType<Animal>().ToList();
        }

        public List<Coral> GetCorals(String aquarium)
        {
            return Collection.AsQueryable<AquariumItem>().Where(x => x.Aquarium.Equals(aquarium)).OfType<Coral>().ToList();
        }
    }
}
