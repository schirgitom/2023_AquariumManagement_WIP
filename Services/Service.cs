using DAL;
using DAL.Entities;
using DAL.Repository;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Services.Models.Response;
using Services.Utils;
using Utils;

namespace Services
{
    public abstract class Service<TEntity> where TEntity : Entity
    {
        protected UnitOfWork UnitOfWork;
        protected IRepository<TEntity> repository;
        protected GlobalService globalService;
        protected IModelStateWrapper validationDictionary;
        protected ModelStateDictionary Validation = null;
        protected User CurrentUser { get; private set; }

        protected Serilog.ILogger log = Logger.ContextLog<Service<TEntity>>();

        public Service(UnitOfWork uow, IRepository<TEntity> repository, GlobalService service)
        {
            UnitOfWork = uow;
            this.repository = repository;
            this.globalService = service;
        }

        public virtual async Task<ActionResponseModel> Delete(String id)
        {
            await repository.DeleteByIdAsync(id);
            ActionResponseModel returnval = new ActionResponseModel();
            returnval.Success = true;

            return returnval;
        }

        public async Task Load(String user)
        {
            CurrentUser = await UnitOfWork.Users.FindOneAsync(x => x.Email.ToLower().Equals(user.ToLower()));


        }

        protected abstract Task<ItemResponseModel<TEntity>> Create(TEntity entity);

        public abstract Task<ItemResponseModel<TEntity>> Update(String id, TEntity entity);

        public abstract Task<Boolean> Validate(TEntity entity);

        public virtual async Task<ItemResponseModel<TEntity>> CreateHandler(TEntity entity)
        {
            ItemResponseModel<TEntity> response = new ItemResponseModel<TEntity>();

            if (await Validate(entity))
            {
                ItemResponseModel<TEntity> ent = await Create(entity);

                if (ent != null)
                {
                    return ent;
                }
                else
                {
                    response.HasError = true;
                    response.Data = null;
                    response.ErrorMessages.Add("Object was empty");
                }
            }
            else
            {
                response.HasError = true;
                response.ErrorMessages = validationDictionary.Errors.Values.ToList();
            }

            return response;
        }

        public virtual async Task<ItemResponseModel<TEntity>> UpdateHandler(String id, TEntity entity)
        {
            ItemResponseModel<TEntity> response = new ItemResponseModel<TEntity>();

            if (await Validate(entity))
            {
                ItemResponseModel<TEntity> ent = await Update(id, entity);

                if (ent != null)
                {
                    if (ent.HasError == false)
                    {
                        ent.Data.ID = id;
                        await this.repository.UpdateOneAsync(ent.Data);
                        response.Data = ent.Data;
                        response.HasError = false;
                    }
                    else
                    {
                        return ent;
                    }
                }
                else
                {
                    response.HasError = true;
                    response.Data = null;
                    response.ErrorMessages.Add("Object was empty");
                }
            }
            else
            {
                response.HasError = true;
                response.ErrorMessages = validationDictionary.Errors.Values.ToList();
            }

            return response;
        }

        public async virtual Task<TEntity> Get(String id)
        {
            return await repository.FindByIdAsync(id);
        }

        public async virtual Task<List<TEntity>> Get()
        {
            return this.repository.FilterBy(x => true);
        }

        public async Task SetModelState(ModelStateDictionary validate)
        {
            validationDictionary = new ModelStateWrapper(validate);
            this.Validation = validate;
        }


    }
}
