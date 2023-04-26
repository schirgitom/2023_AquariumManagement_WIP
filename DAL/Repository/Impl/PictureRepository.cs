using DAL.Entities;
using DAL.UnitOfWork;

namespace DAL.Repository.Impl
{
    public class PictureRepository : Repository<Picture>, IPictureRepository
    {
        public PictureRepository(DBContext context) : base(context)
        {

        }
    }
}
