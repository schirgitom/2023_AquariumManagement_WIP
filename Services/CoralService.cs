using DAL;
using DAL.Entities;
using DAL.Repository.Impl;
using Services.Models.Response;

namespace Services
{
    public class CoralService : AquariumItemService
    {
        public CoralService(UnitOfWork uow, IAquariumItemRepository repo, GlobalService service) : base(uow, repo, service)
        {
        }

        public override async Task<bool> Validate(AquariumItem entry)
        {
            if (entry.GetType() == typeof(Coral))
            {
                return await base.Validate(entry);
            }
            else
            {
                validationDictionary.AddError("NotValid", "Item is no Coral");
            }

            return validationDictionary.IsValid;
        }


        public async Task<ItemResponseModel<Coral>> AddCoral(String id, Coral entry)
        {
            ItemResponseModel<Coral> coralresp = new ItemResponseModel<Coral>();
            ItemResponseModel<AquariumItem> resp = await base.AddAquariumItem(id, entry);

            if (resp.HasError == false)
            {
                coralresp.Data = resp.Data as Coral;
            }
            else
            {
                coralresp.AddErrorMessageRange(resp.ErrorMessages);
                coralresp.AddWarningMessageRange(resp.WarningMessages);
                coralresp.HasError = true;
            }
            return coralresp;
        }


        public async Task<List<Coral>> GetCorals(String id)
        {
            return Repository.GetCorals(id);
        }
    }
}
