using DAL;
using DAL.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Moq;
using Services;
using Services.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.ServiceTest
{
    public class UserServiceTest
    {
        UnitOfWork UnitOfWork = new UnitOfWork();
        public UserServiceTest() { }

        [Test]
        public async Task TestInsert()
        {
            UserServices userservice = new UserServices(UnitOfWork, UnitOfWork.Users, null);

            User usr = new User();
            usr.IsActive = true;
            usr.Lastname = "Schirgi";
            usr.Firstname = "Moritz";
            usr.Email = "thomas.schirgi@schischo.com";
            usr.Password = "12345";

            var modelState = new Mock<ModelStateDictionary>();
            await userservice.SetModelState(modelState.Object);

            ItemResponseModel<User> fromservice = await userservice.CreateHandler(usr);

            Assert.NotNull(fromservice);
            Assert.IsFalse(fromservice.HasError);


        }


        [Test]
        public async Task TestInsertFailed()
        {
            UserServices userservice = new UserServices(UnitOfWork, UnitOfWork.Users, null);

            User usr = new User();
            usr.IsActive = true;
            usr.Lastname = "Schirgi";
            usr.Email = "thomas.schirgi@schischo.com";
            usr.Password = "12345";

            var modelState = new Mock<ModelStateDictionary>();
            await userservice.SetModelState(modelState.Object);

            ItemResponseModel<User> fromservice = await userservice.CreateHandler(usr);

            Assert.NotNull(fromservice);
            Assert.IsTrue(fromservice.HasError);


        }

    }
}
