using DAL.Entities;

namespace DAL.Repository.Impl
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> Login(String username, String password);
    }
}
