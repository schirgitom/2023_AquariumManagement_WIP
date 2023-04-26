using DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using System.Security.Claims;
using Utils;

namespace AquariumManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseController<T> : ControllerBase where T : Entity
    {
        protected Service<T> _Service = null;
        protected Serilog.ILogger log = Logger.ContextLog<BaseController<T>>();
        protected String UserEmail = null;
        protected ClaimsPrincipal ClaimsPrincipal = null;
        public BaseController(Service<T> service, IHttpContextAccessor accessor)
        {

            this._Service = service;
            //this._Service.Load(this.User);
            Task model = this._Service.SetModelState(this.ModelState);
            model.Wait();

            if (accessor != null)
            {
                if (accessor.HttpContext != null)
                {
                    if (accessor.HttpContext.User != null)
                    {
                        ClaimsPrincipal = accessor.HttpContext.User;

                        var identity = (ClaimsIdentity)accessor.HttpContext.User.Identity;

                        if (identity != null)
                        {
                            IEnumerable<Claim> claims = identity.Claims;

                            var email = identity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);

                            if (email != null)
                            {
                                UserEmail = email.Value;
                                Task tsk = this._Service.Load(UserEmail);
                                tsk.Wait();
                            }
                        }
                        else
                        {
                            log.Debug("Identity is null");
                        }

                    }
                    else
                    {
                        log.Debug("Http Context User is null");
                    }
                }
                else
                {
                    log.Debug("HTTP Context is null");
                }
            }
            else
            {
                log.Debug("Accessor is null");
            }

            // this._Service.Load(ClaimsPrincipal);
            //return email.FirstOrDefault().ToString();


        }

        [HttpGet("All")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<ActionResult<List<T>>> GetAll()
        {
            List<T> result = await _Service.Get();

            if (result == null || result.Count == 0)
            {
                return new NotFoundObjectResult(null);
            }

            return result;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize]
        public virtual async Task<ActionResult<T>> Get(String id)
        {
            T result = await _Service.Get(id);

            if (result == null)
            {
                return new NotFoundObjectResult(null);
            }

            return result;
        }
    }
}
