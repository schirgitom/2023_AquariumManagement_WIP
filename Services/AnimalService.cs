using DAL;
using DAL.Entities;
using DAL.Repository.Impl;
using Services.Models.Response;

namespace Services
{
    public class AnimalService : AquariumItemService
    {
        public AnimalService(UnitOfWork uow, IAquariumItemRepository repo, GlobalService service) : base(uow, repo, service)
        {
        }

        public override async Task<bool> Validate(AquariumItem entry)
        {
            if (entry.GetType() == typeof(Animal))
            {
                Boolean basis = await base.Validate(entry);


                return basis;
            }
            else
            {
                modelStateWrapper.AddError("NotValid", "Item is no Coral");
            }

            return modelStateWrapper.IsValid;
        }


        public async Task<ItemResponseModel<Animal>> AddAnimal(String id, Animal entry)
        {
            ItemResponseModel<Animal> coralresp = new ItemResponseModel<Animal>();

            //if (entry.DeathDate == DateTime.MinValue)
            //{
            //    entry.IsAlive = true;
            //}
            //else
            //{
            //    entry.IsAlive = false;
            //}

            ItemResponseModel<AquariumItem> resp = await base.AddAquariumItem(id, entry);

            if (resp.HasError == false)
            {
                coralresp.Data = resp.Data as Animal;
            }
            else
            {
                coralresp.ErrorMessages = resp.ErrorMessages;
                coralresp.HasError = true;
            }
            return coralresp;
        }


        public async Task<List<Animal>> GetAnimals(String aquarium)
        {
            return Repository.GetAnimals(aquarium);
        }
    }
}
