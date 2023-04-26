using DAL.Entities;
using DAL.Repository.Impl;
using DAL.UnitOfWork;
using Services.Models.Response.Basis;

namespace Services
{
    public class AquariumItemService : Service<AquariumItem>
    {
        protected IAquariumItemRepository Repository { get; set; }

        public AquariumItemService(UnitOfWork uow, IAquariumItemRepository repo, GlobalService service) : base(uow, repo, service)
        {
            Repository = repo;
        }

        public override async Task<ItemResponseModel<AquariumItem>> Update(string id, AquariumItem entry)
        {
            ItemResponseModel<AquariumItem> ret = new ItemResponseModel<AquariumItem>();
            ret.HasError = true;
            AquariumItem anf = await UnitOfWork.AquariumItems.FindByIdAsync(id);

            if (anf != null)
            {
                entry.ID = id;
                ret.Data = entry;
                ret.HasError = false;
            }
            return ret;
        }

        public override async Task<bool> Validate(AquariumItem entry)
        {
            if (entry != null)
            {
                if (entry.Inserted <= DateTime.MinValue)
                {
                    validationDictionary.AddError("InsertedMissing", "No insert date was set");
                }
                if (entry.Amount <= 0)
                {
                    validationDictionary.AddError("AmountMissing", "Amount must be greater 0");
                }
            }
            else
            {
                validationDictionary.AddError("ItemEmpty", "Object is empty");
            }

            return validationDictionary.IsValid;

        }

        protected override async Task<ItemResponseModel<AquariumItem>> Create(AquariumItem entry)
        {
            ItemResponseModel<AquariumItem> ret = new ItemResponseModel<AquariumItem>();
            AquariumItem data = await Repository.InsertOneAsync(entry);

            ret.Data = data;
            ret.HasError = false;
            return ret;
        }

        protected virtual async Task<ItemResponseModel<AquariumItem>> AddAquariumItem(String id, AquariumItem item)
        {
            ItemResponseModel<AquariumItem> ret = new ItemResponseModel<AquariumItem>();
            Aquarium aquarium = await UnitOfWork.Aquariums.FindOneAsync(x => x.Name.Equals(id));

            if (aquarium != null)
            {
                item.Aquarium = aquarium.Name;
                ret = await CreateHandler(item);
                return ret;
            }
            else
            {
                ret.HasError = true;
                ret.ErrorMessages.Add("NotFound", "Aquarium not found");
            }

            return ret;
        }






    }
}
