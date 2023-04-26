using DAL.UnitOfWork;

namespace Services
{
    public class GlobalService
    {
        public AquariumService AquariumService { get; set; }
        public UserService UserService { get; set; }
        public CoralService CoralService { get; set; }

        public PictureService PictureService { get; set; }
        public AnimalService AnimalService { get; set; }
        public UnitOfWork UnitOfWork { get; set; }

        public GlobalService(IUnitOfWork UnitOfWork)
        {
            UnitOfWork uow = (UnitOfWork)UnitOfWork;

            this.UnitOfWork = uow;

            AquariumService = new AquariumService(uow, uow.Aquariums, this);
            UserService = new UserService(uow, uow.Users, this);
            CoralService = new CoralService(uow, uow.AquariumItems, this);
            AnimalService = new AnimalService(uow, uow.AquariumItems, this);
            PictureService = new PictureService(uow, uow.Pictures, this);
        }
    }
}
