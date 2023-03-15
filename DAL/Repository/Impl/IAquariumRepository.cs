using DAL.Entities;

namespace DAL.Repository.Impl
{
    public interface IAquariumRepository : IRepository<Aquarium>
    {
        Task<Aquarium> GetByName(string name);
    }
}
