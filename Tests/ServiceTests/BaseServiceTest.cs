using Microsoft.AspNetCore.Http;
using Moq;
using System.Security.Claims;

namespace Tests.ServiceTests
{
    public class BaseServiceTest
    {



        public IHttpContextAccessor Create(ClaimsPrincipal c)
        {
            var mock = new Mock<IHttpContextAccessor>();

            mock.Setup(o => o.HttpContext.User).Returns(c);

            return mock.Object;
        }


        public ClaimsPrincipal Login(string username)
        {

            Claim claim = new Claim(ClaimTypes.Email, username);
            List<Claim> claims = new List<Claim>();
            claims.Add(claim);
            ClaimsIdentity id = new ClaimsIdentity(claims);

            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(id);

            return claimsPrincipal;
        }

    }
}
