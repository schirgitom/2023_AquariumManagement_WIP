using DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Services;
using Services.Models.Request;
using Services.Models.Response;
using Services.Models.Response.Basis;
using System.ComponentModel.DataAnnotations;

namespace AquariumManagementAPI.Controllers
{
    public class AquariumController : BaseController<Aquarium>
    {
        AquariumService AquariumService;
        CoralService CoralService;
        GlobalService Service;
        AnimalService AnimalService;
        PictureService PictureService;
        public AquariumController(GlobalService service, IHttpContextAccessor accessor) : base(service.AquariumService, accessor)
        {
            AquariumService = service.AquariumService;
            CoralService = service.CoralService;
            AnimalService = service.AnimalService;
            this.Service = service;
            PictureService = service.PictureService;

            service.CoralService.SetModelState(this.ModelState);
            service.AnimalService.SetModelState(this.ModelState);
        }





        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize]
        public override async Task<ActionResult<Aquarium>> Get(string id)
        {
            Aquarium aquarium = await AquariumService.Get(id);

            if (aquarium == null)
            {
                return new NotFoundObjectResult(null);
            }

            return aquarium;


        }



        [HttpPost]
        [Authorize]
        // [Route("Aquarium")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ItemResponseModel<Aquarium>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public virtual async Task<ActionResult<ItemResponseModel<Aquarium>>> Create([Required][FromBody] Aquarium aquarium)
        {
            ActionResult<ItemResponseModel<Aquarium>> respobse = AquariumService.CreateHandler(aquarium).ToResponse();

            return respobse;
        }


        [HttpPatch]
        [Authorize]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ItemResponseModel<Aquarium>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public virtual async Task<ActionResult<ItemResponseModel<Aquarium>>> Edit([Required] String id, [Required][FromBody] Aquarium aquarium)
        {
            ActionResult<ItemResponseModel<Aquarium>> respobse = AquariumService.UpdateHandler(id, aquarium).ToResponse();

            return respobse;
        }


        [HttpPost]
        [Authorize]
        [Route("{id}/Coral")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ItemResponseModel<Coral>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public virtual async Task<ActionResult<ItemResponseModel<Coral>>> Coral([Required] String id, [Required][FromBody] Coral coral)
        {

            ItemResponseModel<Coral> response = await CoralService.AddCoral(id, coral);

            ActionResult<ItemResponseModel<Coral>> actresp = response.ToResponse();

            return actresp;
        }


        [HttpPut]
        [Authorize]
        [Route("{id}/Coral/{CoralID}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ItemResponseModel<Coral>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public virtual async Task<ActionResult<ItemResponseModel<AquariumItem>>> Coral([Required] String id, [Required] String CoralID, [Required][FromBody] Coral coral)
        {

            ItemResponseModel<AquariumItem> response = await CoralService.UpdateHandler(CoralID, coral);

            ActionResult<ItemResponseModel<AquariumItem>> actresp = response.ToResponse();

            return actresp;


        }


        [HttpPost]
        [Authorize]
        [Route("{id}/Animal")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ItemResponseModel<Animal>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public virtual async Task<ActionResult<ItemResponseModel<Animal>>> Animal([Required] String id, [Required][FromBody] Animal animal)
        {

            ItemResponseModel<Animal> response = await AnimalService.AddAnimal(id, animal);

            ActionResult<ItemResponseModel<Animal>> actresp = response.ToResponse();

            return actresp;


        }

        [HttpPut]
        [Authorize]
        [Route("{id}/Animal/{AnimalID}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ItemResponseModel<Animal>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public virtual async Task<ActionResult<ItemResponseModel<AquariumItem>>> Animal([Required] String id, [Required] String AnimalID, [Required][FromBody] Animal animal)
        {

            ItemResponseModel<AquariumItem> response = await AnimalService.UpdateHandler(AnimalID, animal);

            ActionResult<ItemResponseModel<AquariumItem>> actresp = response.ToResponse();

            return actresp;


        }

        [HttpGet]
        [Authorize]
        [Route("ForUser")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<AquariumUserResponse>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public virtual async Task<ActionResult<List<AquariumUserResponse>>> ForUser()
        {

            return await AquariumService.GetForUser();


        }



        [HttpPost]
        [Authorize]
        [Route("{id}/Picture")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ItemResponseModel<PictureResponse>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public virtual async Task<ActionResult<ItemResponseModel<PictureResponse>>> Picture([Required] String id, [FromForm] PictureRequest Picture)
        {
            ItemResponseModel<PictureResponse> response = await PictureService.AddPicture(id, Picture);

            if (Response != null && response.HasError == false)
            {
                return new OkObjectResult(response);
            }

            return new BadRequestObjectResult(response);
        }


        [HttpDelete]
        [Authorize]
        [Route("{id}/Picture/{PictureID}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ItemResultModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public virtual async Task<ActionResult<ItemResultModel>> Picture([Required] String id, [Required] String PictureID)
        {
            ItemResultModel response = await PictureService.Delete(id, PictureID);

            if (Response != null && response.HasError == false && response.Success == true)
            {
                return new OkObjectResult(response);
            }

            return new BadRequestObjectResult(response);
        }

        [HttpGet]
        [Authorize]
        [Route("{id}/Picture/{PictureID}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ItemResponseModel<PictureResponse>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public virtual async Task<ActionResult<ItemResponseModel<PictureResponse>>> GetPicture([Required] String id, [Required] String PictureID)
        {
            ItemResponseModel<PictureResponse> response = await PictureService.GetPicture(PictureID);

            if (response != null && response.HasError == false)
            {
                return new OkObjectResult(response);
            }

            return new BadRequestObjectResult(response);
        }



        [HttpGet]
        [Authorize]
        [Route("{id}/Pictures")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<PictureResponse>))]
        public virtual async Task<ActionResult<List<PictureResponse>>> Picture([Required] String id)
        {
            List<PictureResponse> pics = await PictureService.GetForAquarium(id);

            return pics;
        }


        [HttpGet("{id}/Corals")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Coral>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<List<Coral>> GetCorals(String id)
        {
            List<Coral> reutnval = await CoralService.GetCorals(id);

            return reutnval;
        }

        [HttpGet("{id}/Animals")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Animal>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<List<Animal>> GetAnimals(String id)
        {
            return await AnimalService.GetAnimals(id);
        }

        [HttpGet("{id}/Coral/{CoralId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Coral))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<Coral>> GetCoral(String id, String CoralId)
        {
            AquariumItem item = await CoralService.Get(CoralId);

            Coral coral = item as Coral;

            if (coral != null)
            {
                return coral;
            }


            return BadRequest();
        }



        [HttpGet("{id}/Animal/{AnimalId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Animal))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<Animal>> GetAnimal(String id, String AnimalId)
        {
            AquariumItem item = await AnimalService.Get(AnimalId);

            Animal coral = item as Animal;

            if (coral != null)
            {
                return coral;
            }


            return BadRequest();
        }
    }
}
