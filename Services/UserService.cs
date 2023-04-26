using DAL.Entities;
using DAL.Repository.Impl;
using DAL.UnitOfWork;
using Services.Authentication;
using Services.Models.Request;
using Services.Models.Response;
using Services.Models.Response.Basis;

namespace Services
{
    public class UserService : Service<User>
    {

        IUserRepository Repository;
        public UserService(UnitOfWork uow, IUserRepository repo, GlobalService service) : base(uow, repo, service)
        {
            Repository = repo;
        }

        public override async Task<ItemResponseModel<User>> Update(string id, User entry)
        {
            ItemResponseModel<User> ret = new ItemResponseModel<User>();
            ret.HasError = true;
            User anf = await UnitOfWork.Users.FindByIdAsync(id);

            if (anf != null)
            {
                entry.ID = id;
                ret.Data = entry;
                ret.HasError = false;
            }


            return ret;
        }

        public override async Task<bool> Validate(User entry)
        {
            if (entry != null)
            {

                if (String.IsNullOrEmpty(entry.Email))
                {
                    validationDictionary.AddError("NameMissing", "User name must not be empty");
                }
                else
                {
                    User aqua = await UnitOfWork.Users.FindOneAsync(x => x.Email.Equals(entry.Email));

                    if (aqua != null)
                    {
                        if (!String.IsNullOrEmpty(entry.ID))
                        {
                            if (entry.ID.Equals(aqua.ID) == false)
                            {
                                validationDictionary.AddError("NameTaken", "User name is already taken");
                            }
                        }
                        else
                        {
                            validationDictionary.AddError("NameTaken", "User name is already taken");
                        }
                    }
                }


                if (String.IsNullOrEmpty(entry.Firstname))
                {
                    validationDictionary.AddError("NameMissing", "User Firstname must not be empty");
                }

                if (String.IsNullOrEmpty(entry.Lastname))
                {
                    validationDictionary.AddError("NameMissing", "User Lastname must not be empty");
                }


                if (String.IsNullOrEmpty(entry.Password))
                {
                    validationDictionary.AddError("NameMissing", "User Passwort must not be empty");
                }
            }
            else
            {
                validationDictionary.AddError("ItemEmpty", "Object is empty");
            }

            return validationDictionary.IsValid;

        }

        protected override async Task<ItemResponseModel<User>> Create(User entry)
        {
            ItemResponseModel<User> ret = new ItemResponseModel<User>();
            ret.Data = await Repository.InsertOneAsync(entry);
            ret.HasError = false;
            return ret;
        }

        public async Task<ItemResponseModel<UserResponse>> Login(LoginRequest request)
        {

            ItemResponseModel<UserResponse> response = new ItemResponseModel<UserResponse>();

            User usr = await Repository.Login(request.Username, request.Password);

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
                    response.ErrorMessages.Add("Unknown", "User or Passwort do not match");
                }
            }
            else
            {
                response.HasError = true;
                response.ErrorMessages.Add("Unknown", "User or Passwort do not match");
            }
            return response;
        }

    }
}
