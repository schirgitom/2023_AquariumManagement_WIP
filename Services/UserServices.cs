using DAL;
using DAL.Entities;
using DAL.Repository.Impl;
using Services.Authentication;
using Services.Models.Request;
using Services.Models.Response;

namespace Services
{
    public class UserServices : Service<User>
    {
        IUserRepository Repository;
        public UserServices(UnitOfWork uow, IUserRepository repository, GlobalService service) : base(uow, repository, service)
        {
            Repository = repository;
        }

        protected override async Task<ItemResponseModel<User>> Create(User entity)
        {
            ItemResponseModel<User> response = new ItemResponseModel<User>();
            response.Data = await this.repository.InsertOneAsync(entity);
            response.HasError = false;
            return response;

        }

        public override async Task<ItemResponseModel<User>> Update(string id, User entity)
        {
            ItemResponseModel<User> ret = new ItemResponseModel<User>();
            ret.HasError = true;

            User anf = await UnitOfWork.Users.FindByIdAsync(id);
            if (anf != null)
            {
                entity.ID = id;
                ret.Data = entity;
                ret.HasError = false;
            }

            return ret;

        }

        public override async Task<bool> Validate(User entity)
        {
            if (entity != null)
            {
                if (String.IsNullOrEmpty(entity.Email))
                {
                    validationDictionary.AddError("No Email", "Please provide an email");
                }
                else
                {
                    User usr = await UnitOfWork.Users.FindOneAsync(x => x.Email.Equals(entity.Email));

                    if (usr != null)
                    {
                        if (!String.IsNullOrEmpty(entity.ID))
                        {
                            if (entity.ID.Equals(usr.ID) == false)
                            {
                                validationDictionary.AddError("No Email", "Email already taken");
                            }
                        }
                        else
                        {
                            validationDictionary.AddError("No Email", "Email already taken");
                        }
                    }
                }

                if (String.IsNullOrEmpty(entity.Firstname))
                {
                    validationDictionary.AddError("No Firstname", "Please provide a firstname");
                }

                if (String.IsNullOrEmpty(entity.Lastname))
                {
                    validationDictionary.AddError("No Lastname", "Please provide a lastname");
                }
            }
            else
            {
                validationDictionary.AddError("No User", "no user was provided");
            }

            return validationDictionary.IsValid;
        }

        public async Task<ItemResponseModel<UserResponse>> Login(LoginRequest request)
        {
            ItemResponseModel<UserResponse> response = new ItemResponseModel<UserResponse>();

            User usr = await Repository.Login(request.Email, request.Password);

            if (usr != null)
            {
                Authentication.Authentication auth = new Authentication.Authentication(UnitOfWork);
                AuthenticationInformation token = await auth.Authenticate(usr);

                if (token != null)
                {
                    UserResponse returnmodel = new UserResponse();
                    returnmodel.User = usr;
                    returnmodel.AuthenticationInformation = token;

                    response.Data = returnmodel;
                }
                else
                {
                    response.HasError = true;
                    response.ErrorMessages.Add("User or Passwort do not match");
                }
            }
            else
            {
                response.HasError = true;
                response.ErrorMessages.Add("User or Passwort do not match");
            }
            return response;
        }
    }
}
