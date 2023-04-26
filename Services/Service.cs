using DAL;
using DAL.Entities;
using DAL.Repository;
using DAL.UnitOfWork;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Services.Models.Response.Basis;
using Services.Utils;
using Utils;

namespace Services
{
    public abstract class Service<TEntity> where TEntity : Entity
    {

        protected Serilog.ILogger log = Logger.ContextLog<Service<TEntity>>();

        protected UnitOfWork UnitOfWork = null;
        protected IRepository<TEntity> Repository = null;
        protected ModelStateDictionary Validation = null;
        protected GlobalService GlobalService;

        protected IModelStateWrapper validationDictionary;
        public abstract Task<bool> Validate(TEntity entry);

        protected User CurrentUser { get; private set; }

        protected abstract Task<ItemResponseModel<TEntity>> Create(TEntity entry);
        public abstract Task<ItemResponseModel<TEntity>> Update(string id, TEntity entry);
        public virtual async Task<ActionResultResponseModel> Delete(String id)
        {
            await Repository.DeleteByIdAsync(id);

            ActionResultResponseModel model = new ActionResultResponseModel();
            model.Success = true;

            return model;
        }

        public async Task Load(String email)
        {
            CurrentUser = await UnitOfWork.Users.FindOneAsync(x => x.Email.ToLower().Equals(email.ToLower()));

            if (CurrentUser == null)
            {
                log.Warning("Could not determine User");
            }
        }


        public async Task SetModelState(ModelStateDictionary validation)
        {
            validationDictionary = new ModelStateWrapper(validation);
            this.Validation = validation;
        }

        public Service(UnitOfWork uow, IRepository<TEntity> repo, GlobalService service)
        {
            UnitOfWork = uow;
            Repository = repo;
            GlobalService = service;
        }


        public void ClearValidation()
        {
            if (Validation != null)
            {
                validationDictionary = new ModelStateWrapper(Validation);
            }
        }


        public virtual async Task<ItemResponseModel<TEntity>> CreateHandler(TEntity entry)
        {
            ClearValidation();
            ItemResponseModel<TEntity> returnval = new ItemResponseModel<TEntity>();

            try
            {

                if (await Validate(entry))
                {

                    ItemResponseModel<TEntity> ent = await Create(entry);

                    if (ent != null)
                    {
                        if (ent.HasError == false)
                        {

                            returnval.Data = ent.Data;
                            returnval.HasError = false;
                        }
                        else
                        {
                            return ent;
                        }
                    }
                    else
                    {
                        returnval.Data = default;
                        returnval.HasError = true;
                        returnval.ErrorMessages.Add("Empty", "Object was empty");
                    }

                }
                else
                {
                    returnval.Data = default;
                    returnval.HasError = true;
                    returnval.ErrorMessages = validationDictionary.Errors;
                }
            }
            catch (Exception ex)
            {
                returnval.Data = default;
                returnval.HasError = true;
                returnval.ErrorMessages.Add("Error", "Error during create of element");

                log.Warning("Error during creation");
                log.Debug(ex, "Error during creation");

            }


            return returnval;
        }



        public virtual async Task<ItemResponseModel<TEntity>> UpdateHandler(String id, TEntity entry)
        {
            ClearValidation();
            ItemResponseModel<TEntity> returnval = new ItemResponseModel<TEntity>();
            try
            {
                if (await Validate(entry))
                {
                    try
                    {

                        ItemResponseModel<TEntity> ent = await Update(id, entry);
                        if (ent != null && ent.Data != null)
                        {
                            if (ent.HasError == false)
                            {

                                ent.Data.ID = id;
                                await Repository.UpdateOneAsync(ent.Data);
                                returnval.Data = ent.Data;
                                returnval.HasError = false;

                            }
                            else
                            {
                                return ent;
                            }
                        }
                        else
                        {
                            returnval.Data = default;
                            returnval.HasError = true;
                            returnval.ErrorMessages.Add("Empty", "Object was empty");
                        }
                    }

                    catch (Exception ex)
                    {
                        returnval.Data = default;
                        returnval.HasError = true;
                        returnval.ErrorMessages.Add("Error", "Error during create of element");

                        log.Warning("Error during creation");
                        log.Debug(ex, "Error during creation");

                    }
                }
                else
                {
                    returnval.Data = default;
                    returnval.HasError = true;
                    returnval.ErrorMessages = validationDictionary.Errors;
                }

            }
            catch (Exception ex)
            {
                returnval.Data = default;
                returnval.HasError = true;
                returnval.ErrorMessages.Add("Error", "Error during create of element");

                log.Warning("Error during creation");
                log.Debug(ex, "Error during creation");

            }


            return returnval;
        }


        public async virtual Task<TEntity> Get(String id)
        {
            TEntity ent = await Repository.FindByIdAsync(id);

            return ent;
        }


        public async virtual Task<List<TEntity>> Get()
        {
            List<TEntity> ent = Repository.FilterBy(x => true);

            return ent;
        }
    }
}