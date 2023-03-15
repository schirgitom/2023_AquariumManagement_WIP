using DAL.Entities;

namespace DAL.Repository.Impl
{
    public interface IAquariumItemRepository : IRepository<AquariumItem>
    {
        List<Coral> GetCorals(String aquarium);
        List<Animal> GetAnimals(String aquarium);
    }
}
