using DAL.Entities;
using DAL.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace AquariumManagementAPI.Controllers.Odata
{
    public class AnimalController : ODataController
    {
        private IUnitOfWork _db;


        public AnimalController(IUnitOfWork context)
        {
            _db = context;
        }

        [EnableQuery(MaxTop = 100, MaxExpansionDepth = 10)]
        [Authorize]
        public IActionResult Get()
        {

            List<AquariumItem> db = _db.AquariumItems.FilterBy(x => true);

            List<Animal> corals = new List<Animal>();

            foreach (var item in db)
            {
                if (item.GetType() == typeof(Animal))
                {
                    corals.Add((Animal)item);
                }
            }

            return Ok(corals);
        }

    }
}
