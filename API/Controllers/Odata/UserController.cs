using DAL.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace AquariumManagementAPI.Controllers.Odata
{
    public class UserController : ODataController
    {
        private IUnitOfWork _db;


        public UserController(IUnitOfWork context)
        {
            _db = context;
        }

        [EnableQuery(MaxTop = 100, MaxExpansionDepth = 10)]
        [Authorize]
        public IActionResult Get()
        {
            return Ok(_db.Users.FilterBy(x => true));
        }

    }
}
