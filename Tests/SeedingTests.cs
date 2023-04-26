using Tests.DBTests;
using Tests.ServiceTests;

namespace Tests
{
    public class SeedingTests
    {
        [Test]
        public async Task Seed()
        {
            UserServiceTest user = new UserServiceTest();
            await user.InsertOverService();

            AquariumServiceTests aqu = new AquariumServiceTests();
            await aqu.InsertOverService();

            AquariumItemsTest tiere = new AquariumItemsTest();
            await tiere.CreateAnimal();
            await tiere.CreateCoral();
        }
    }
}
