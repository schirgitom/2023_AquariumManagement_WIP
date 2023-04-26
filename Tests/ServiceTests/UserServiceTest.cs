using DAL.Entities;
using DAL.UnitOfWork;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Moq;
using Services;
using Services.Models.Request;
using Services.Models.Response;
using Services.Models.Response.Basis;

namespace Tests.ServiceTests
{
    public class UserServiceTest
    {
        [Test]
        public async Task InsertOverService()
        {
            User user = new User();
            user.Email = "tom.schirgi@gmail.com";
            user.Password = "12345";
            user.Firstname = "Thomas";
            user.Lastname = "Schirgi";
            user.Active = true;

            UnitOfWork uow = new UnitOfWork();


            UserService service = new UserService(uow, uow.Users, null);

            var modelState = new Mock<ModelStateDictionary>();
            await service.SetModelState(modelState.Object);

            ItemResponseModel<User> model = await service.CreateHandler(user);

            Assert.NotNull(model);
            Assert.NotNull(model.Data);
            Assert.IsFalse(model.HasError);

        }



        [Test]
        public async Task LoginTest()
        {
            LoginRequest user = new LoginRequest();
            user.Username = "tom.schirgi@gmail.com";
            user.Password = "12345";
            UnitOfWork uow = new UnitOfWork();
            UserService service = new UserService(uow, uow.Users, null);

            var modelState = new Mock<ModelStateDictionary>();
            await service.SetModelState(modelState.Object);

            ItemResponseModel<UserResponse> resp = await service.Login(user);

            Assert.NotNull(resp);
            Assert.NotNull(resp.Data);

        }
    }
}
