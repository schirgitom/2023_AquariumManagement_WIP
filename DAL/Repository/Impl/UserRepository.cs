using DAL.DBUtils;
using DAL.Entities;

namespace DAL.Repository.Impl
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        PasswordHasher hasher = new PasswordHasher();
        public UserRepository(DBContext Context) : base(Context)
        {
        }


        public async override Task<User> InsertOneAsync(User document)
        {
            SetUser(document);
            document = await base.InsertOneAsync(document);
            return document;
        }

        public async Task<User> Login(string username, string password)
        {

            User fromdb = await base.FindOneAsync(x => x.Email == username);

            if (fromdb != null)
            {
                Boolean verified = hasher.Check(fromdb.HashedPassword, password);

                if (verified)
                {

                    return fromdb;

                }
                else
                {
                    log.Warning("Cannot login " + username + " Wrong password");
                }

            }
            else
            {
                log.Warning("User " + username + " not available");
            }

            return null;

        }

        public async override Task<User> UpdateOneAsync(User document)
        {
            SetUser(document);
            document = await base.UpdateOneAsync(document);
            return document;
        }


        private void SetUser(User document)
        {
            document.HashedPassword = hasher.Hash(document.Password);

        }
    }
}
