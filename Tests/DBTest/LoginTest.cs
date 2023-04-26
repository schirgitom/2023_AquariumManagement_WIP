using DAL;
using DAL.Entities;
using DAL.UnitOfWork;
using Services.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.DBTest
{
    public class LoginTest
    {
        UnitOfWork uow = new UnitOfWork();


        [Test]
        public async Task CreateUser()
        {
            
            User usr = new User();
            usr.Lastname = "Schirgi";
            usr.Firstname = "Thomas";
            usr.Email = "thomas.schirgi@schischo.com";
            usr.Password = "12345";
            await uow.Users.InsertOneAsync(usr);
        }

        [Test]
        public async Task Login()
        {
            User usr = await uow.Users.Login("thomas.schirgi@schischo.com", "12345");

            Authentication auth = new Authentication(uow);
            AuthenticationInformation info = await auth.Authenticate(usr);

            Assert.NotNull(info);

        }
    }
}
