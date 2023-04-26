using DAL.Entities;
using DAL.UnitOfWork;

namespace Tests.DBTests
{
    public class AquariumItemsTest
    {

        [Test]
        public async Task CreateCoral()
        {
            Coral coral = new Coral();
            coral.Amount = 1;
            coral.Aquarium = "SchiScho";
            coral.CoralType = CoralType.HardCoral;
            coral.Description = "Zoa";
            coral.Name = "Zoa";
            coral.Species = "Zoanthus";

            UnitOfWork uow = new UnitOfWork();

            AquariumItem fromdb = await uow.AquariumItems.InsertOneAsync(coral);


            Assert.NotNull(fromdb);
        }



        [Test]
        public async Task CreateAnimal()
        {
            Animal coral = new Animal();
            coral.Amount = 1;
            coral.Aquarium = "SchiScho";
            coral.Description = "Nemo";
            coral.Name = "Nemo";
            coral.Species = "Clown";

            UnitOfWork uow = new UnitOfWork();

            AquariumItem fromdb = await uow.AquariumItems.InsertOneAsync(coral);


            Assert.NotNull(fromdb);
        }



        [Test]
        public async Task GetCorals()
        {

            UnitOfWork uow = new UnitOfWork();

            List<Coral> fromdb = uow.AquariumItems.GetCorals("safasf");


            Assert.NotNull(fromdb);
            Assert.Greater(fromdb.Count, 0);
        }
    }
}
