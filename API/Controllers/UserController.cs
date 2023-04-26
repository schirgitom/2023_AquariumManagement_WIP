using DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Models.Request;
using Services.Models.Response;
using Services.Models.Response.Basis;
using System.ComponentModel.DataAnnotations;

namespace AquariumManagementAPI.Controllers
{
    public class UserController : BaseController<User>
    {

        UserService userService;
        public UserController(GlobalService service, IHttpContextAccessor accessor) : base(service.UserService, accessor)
        {
            userService = service.UserService;
        }



        [HttpPost]
        [Route("Login")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ItemResponseModel<UserResponse>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public virtual async Task<ActionResult<ItemResponseModel<UserResponse>>> Login([Required][FromBody] LoginRequest cred)
        {
            ItemResponseModel<UserResponse> login = await userService.Login(cred);

            if (login != null && login.HasError == false)
            {
                return login;
            }
            else
            {
                login = new ItemResponseModel<UserResponse>();
                login.HasError = true;
                login.ErrorMessages.Add("LoginFailed", "Login Failed");
            }
            return new UnauthorizedObjectResult(login);
        }



        [HttpPost("Register")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(User))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ItemResponseModel<User>>> Register([Required][FromBody] User cred)
        {
            return await userService.CreateHandler(cred);

        }



        [HttpPatch("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(User))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ItemResponseModel<User>>> UpdateUser([Required] String id, [Required][FromBody] User cred)
        {
            return await userService.UpdateHandler(id, cred);

        }


    }
}
