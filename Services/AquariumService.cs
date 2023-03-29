using DAL;
using DAL.Entities;
using DAL.Repository.Impl;
using Services.Models.Response;

namespace Services
{
    public class AquariumService : Service<Aquarium>
    {
        IAquariumRepository Repository;

        public AquariumService(UnitOfWork uow, IAquariumRepository repo, GlobalService service) : base(uow, repo, service)
        {
            Repository = repo;
        }

        public override async Task<ItemResponseModel<Aquarium>> Update(string id, Aquarium entry)
        {
            ItemResponseModel<Aquarium> ret = new ItemResponseModel<Aquarium>();
            ret.HasError = true;

            Aquarium anf = await UnitOfWork.Aquariums.FindByIdAsync(id);

            if (anf != null)
            {
                entry.ID = id;
                entry.Liters = (entry.Depth * entry.Height * entry.Length) * 0.001;
                ret.Data = entry;
                ret.HasError = false;
            }


            return ret;
        }

        public override async Task<bool> Validate(Aquarium entry)
        {
            if (entry != null)
            {
                if (entry.Length <= 0)
                {
                    modelStateWrapper.AddError("LengthMissing", "Aquarium lenght must be greater 0");
                }
                if (entry.Height <= 0)
                {
                    modelStateWrapper.AddError("HeightMissing", "Aquarium height must be greater 0");
                }
                if (entry.Depth <= 0)
                {
                    modelStateWrapper.AddError("DepthMissing", "Aquarium depth must be greater 0");
                }

                if (String.IsNullOrEmpty(entry.Name))
                {
                    modelStateWrapper.AddError("NameMissing", "Aquarium name must not be empty");
                }
                else
                {
                    Aquarium aqua = await UnitOfWork.Aquariums.GetByName(entry.Name);

                    if (aqua != null)
                    {
                        if (!String.IsNullOrEmpty(entry.ID))
                        {
                            if (entry.ID.Equals(aqua.ID) == false)
                            {
                                modelStateWrapper.AddError("NameTaken", "Aquarium name is already taken");
                            }
                        }
                        else
                        {
                            modelStateWrapper.AddError("NameTaken", "Aquarium name is already taken");
                        }
                    }
                }
            }
            else
            {
                modelStateWrapper.AddError("ItemEmpty", "Object is empty");
            }

            return modelStateWrapper.IsValid;

        }

        public override async Task<Aquarium> Get(String id)
        {
            Aquarium aquarium = await Repository.FindByIdAsync(id);

            if (aquarium != null)
            {
                List<UserAquarium> userAquaria = await UnitOfWork.UsersAquarium.FilterByAsync(x => x.UserID.Equals(CurrentUser.ID) && x.AquariumID.Equals(aquarium.ID));

                if (userAquaria != null && userAquaria.Count > 0)
                {
                    return aquarium;
                }
            }

            return null;

        }

        protected override async Task<ItemResponseModel<Aquarium>> Create(Aquarium entry)
        {
            ItemResponseModel<Aquarium> ret = new ItemResponseModel<Aquarium>();

            entry.Liters = (entry.Depth * entry.Height * entry.Length) * 0.001;


            Aquarium data = await Repository.InsertOneAsync(entry);



            UserAquarium user = new UserAquarium();

            user.UserID = CurrentUser.ID;
            user.AquariumID = data.ID;
            user.Role = UserRole.Admin;

            await UnitOfWork.UsersAquarium.InsertOneAsync(user);


            ret.Data = data;
            ret.HasError = false;
            return ret;
        }


        public async Task<List<AquariumUserResponse>> GetForUser()
        {
            List<UserAquarium> userAquaria = await UnitOfWork.UsersAquarium.FilterByAsync(x => x.UserID.Equals(CurrentUser.ID));

            List<AquariumUserResponse> returnavl = new List<AquariumUserResponse>();

            foreach (UserAquarium aqua in userAquaria)
            {
                Aquarium aq = await Repository.FindByIdAsync(aqua.AquariumID);
                AquariumUserResponse resp = new AquariumUserResponse();

                resp.Aquarium = aq;
                resp.Role = aqua.Role;

                returnavl.Add(resp);

            }

            return returnavl;
        }






    }
}
