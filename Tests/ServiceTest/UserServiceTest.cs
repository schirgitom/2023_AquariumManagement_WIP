using DAL;
using DAL.Entities;
using DAL.UnitOfWork;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Moq;
using Services;
using Services.Models.Response;
using Services.Models.Response.Basis;
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
            UserService userservice = new UserService(UnitOfWork, UnitOfWork.Users, null);

            User usr = new User();
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
            UserService userservice = new UserService(UnitOfWork, UnitOfWork.Users, null);

            User usr = new User();
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
