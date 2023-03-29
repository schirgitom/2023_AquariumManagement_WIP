using DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Models.Request;
using Services.Models.Response;

namespace API.Controllers
{
    public class UserController : BaseController<User>
    {
        UserServices UserServices = null;
        public UserController(GlobalService service, IHttpContextAccessor accessor) : base(service.UserService, accessor)
        {

        }

        [HttpPost]
        public async Task<ActionResult<ItemResponseModel<UserResponse>>> Login([FromBody]LoginRequest request)
        {
            return UserServices.Login();
        }
    }
}
