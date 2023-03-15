using DAL;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.DBTest
{
    public class AquariumTests
    {
        [Test]
        public async Task CreateAquarium()
        {
            Aquarium aquarium = new Aquarium();
            aquarium.Liters = 500;
            aquarium.Height = 55;
            aquarium.Depth = 65;
            aquarium.Length = 150;
            aquarium.WaterType = WaterType.Saltwater;
            aquarium.Name = "SchiScho";

            UnitOfWork uow = new UnitOfWork();

            Aquarium fromdb = await uow.Aquarium.InsertOneAsync(aquarium);

            Assert.NotNull(fromdb);
            Assert.NotNull(fromdb.ID);


        }
    }
}
