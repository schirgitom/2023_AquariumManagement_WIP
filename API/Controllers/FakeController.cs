using DAL.Entities;
using DAL.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace AquariumManagementAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class FakeController : ControllerBase
    {
        [HttpGet("Seed")]
        public async Task<ActionResult<Boolean>> Seed()
        {
            await Aquarium();
            return true;
        }






        private async Task Aquarium()
        {
            User user = new User();
            user.Email = "user@aquarium.com";
            user.Password = "12345";
            user.Firstname = "Hey";
            user.Lastname = "Dude";
            user.Active = true;

            UnitOfWork uow = new UnitOfWork();

            await uow.Users.InsertOneAsync(user);


            Aquarium aquarium = new Aquarium();
            aquarium.Depth = 65;
            aquarium.Height = 55;
            aquarium.Liters = 472;
            aquarium.Length = 150;
            aquarium.Name = "SchiScho";
            aquarium.WaterType = WaterType.Saltwater;



            //AquariumService service = new AquariumService(uow, uow.Aquariums, null);
            //await service.Load("user@aquarium.com");


            await uow.Aquariums.InsertOneAsync(aquarium);

            UserAquarium aquu = new UserAquarium();
            aquu.AquariumID = aquarium.ID;
            aquu.UserID = user.ID;
            aquu.Role = UserRole.Admin;

            await uow.UsersAquarium.InsertOrUpdateOneAsync(aquu);


            Coral coral = new Coral();
            coral.Amount = 1;
            coral.Aquarium = "SchiScho";
            coral.CoralType = CoralType.HardCoral;
            coral.Description = "Zoa";
            coral.Name = "Zoa";
            coral.Species = "Zoanthus";


            AquariumItem fromdb = await uow.AquariumItems.InsertOneAsync(coral);

            Animal animal = new Animal();
            animal.Amount = 1;
            animal.Aquarium = "SchiScho";
            animal.Description = "Nemo";
            animal.Name = "Nemo";
            animal.Species = "Clown";
            animal.Inserted = DateTime.Now;
            animal.IsAlive = true;

            AquariumItem afromdb = await uow.AquariumItems.InsertOneAsync(animal);





        }






    }
}
