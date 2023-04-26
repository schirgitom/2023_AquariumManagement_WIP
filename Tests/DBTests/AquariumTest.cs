using DAL.Entities;
using DAL.UnitOfWork;

namespace Tests.DBTests
{
    public class AquariumTest
    {
        [Test]
        public async Task CreateAquarium()
        {
            Aquarium aquarium = new Aquarium();
            aquarium.Depth = 65;
            aquarium.Height = 55;
            aquarium.Length = 150;
            aquarium.Name = "SchiScho";
            aquarium.WaterType = WaterType.Saltwater;

            UnitOfWork uow = new UnitOfWork();

            Aquarium fromdb = await uow.Aquariums.InsertOneAsync(aquarium);


            Assert.NotNull(fromdb);

        }
    }
}
