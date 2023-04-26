using DAL.Entities;
using DAL.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Moq;
using Services;
using Services.Models.Request;
using Services.Models.Response;
using Services.Models.Response.Basis;

namespace Tests.ServiceTests
{
    public class AquariumServiceTests
    {
        //https://mongodb.github.io/mongo-csharp-driver/2.13/reference/gridfs/deletingandrenamingfiles/

        [Test]
        public async Task InsertOverService()
        {
            Aquarium aquarium = new Aquarium();
            aquarium.Depth = 65;
            aquarium.Height = 55;
            aquarium.Length = 150;
            aquarium.Name = "SchiScho2";
            aquarium.WaterType = WaterType.Saltwater;

            UnitOfWork uow = new UnitOfWork();


            AquariumService service = new AquariumService(uow, uow.Aquariums, null);
            await service.Load("tom.schirgi@gmail.com");

            var modelState = new Mock<ModelStateDictionary>();
            await service.SetModelState(modelState.Object);

            ItemResponseModel<Aquarium> model = await service.CreateHandler(aquarium);

            Assert.NotNull(model);
            Assert.NotNull(model.Data);
            Assert.IsFalse(model.HasError);

        }


        [Test]
        public async Task UpdateOverService()
        {
            Aquarium aquarium = new Aquarium();
            aquarium.Depth = 65;
            aquarium.Height = 55;
            aquarium.Length = 150;
            aquarium.Name = "SchiScho3";
            aquarium.WaterType = WaterType.Saltwater;

            UnitOfWork uow = new UnitOfWork();


            AquariumService service = new AquariumService(uow, uow.Aquariums, null);

            var modelState = new Mock<ModelStateDictionary>();
            await service.SetModelState(modelState.Object);

            ItemResponseModel<Aquarium> model = await service.CreateHandler(aquarium);

            Assert.NotNull(model);
            Assert.NotNull(model.Data);
            Assert.IsFalse(model.HasError);


            Aquarium fromdb = model.Data;

            fromdb.WaterType = WaterType.Freshwater;


            ItemResponseModel<Aquarium> model2 = await service.UpdateHandler(fromdb.ID, fromdb);

            Assert.NotNull(model2);
            Assert.NotNull(model2.Data);
            Assert.IsFalse(model2.HasError);

        }


        [Test]
        public async Task FailInsertOverService()
        {
            String unique = Guid.NewGuid().ToString();

            Aquarium aquarium = new Aquarium();
            aquarium.Depth = 65;
            aquarium.Height = 55;
            aquarium.Length = 150;
            aquarium.Name = unique;
            aquarium.WaterType = WaterType.Saltwater;

            UnitOfWork uow = new UnitOfWork();


            AquariumService service = new AquariumService(uow, uow.Aquariums, null);

            var modelState = new Mock<ModelStateDictionary>();
            await service.SetModelState(modelState.Object);

            ItemResponseModel<Aquarium> model = await service.CreateHandler(aquarium);

            Assert.NotNull(model);
            Assert.NotNull(model.Data);
            Assert.IsFalse(model.HasError);



            Aquarium aquarium2 = new Aquarium();
            aquarium2.Depth = 65;
            aquarium2.Height = 55;
            aquarium2.Length = 150;
            aquarium2.Name = unique;
            aquarium2.WaterType = WaterType.Saltwater;



            ItemResponseModel<Aquarium> model2 = await service.CreateHandler(aquarium2);

            Assert.NotNull(model2);
            Assert.Null(model2.Data);
            Assert.IsTrue(model2.HasError);

        }



        [Test]
        public async Task UploadImage()
        {
            String unique = "SchiScho2";

            Aquarium aquarium = new Aquarium();
            aquarium.Depth = 65;
            aquarium.Height = 55;
            aquarium.Length = 150;
            aquarium.Name = unique;
            aquarium.WaterType = WaterType.Saltwater;

            UnitOfWork uow = new UnitOfWork();


            AquariumService service = new AquariumService(uow, uow.Aquariums, null);
            await service.Load("tom.schirgi@gmail.com");
            var modelState = new Mock<ModelStateDictionary>();
            await service.SetModelState(modelState.Object);


            byte[] bytes = System.IO.File.ReadAllBytes("./TestImages/1.jpg");

            IFormFile file = new FormFile(new MemoryStream(bytes), 0, bytes.Length, "Data", "1.jpg");

            PictureRequest req = new PictureRequest();
            req.Description = "Ein Bild";
            req.FormFile = file;

            PictureService pservice = new PictureService(uow, uow.Pictures, null);
            await pservice.Load("tom.schirgi@gmail.com");
            ItemResponseModel<PictureResponse> resp = await pservice.AddPicture(unique, req);

            Assert.NotNull(resp);
            Assert.IsFalse(resp.HasError);



        }
        [Test]
        public async Task DownloadImagfe()
        {
            UnitOfWork uow = new UnitOfWork();


            PictureService service = new PictureService(uow, uow.Pictures, null);
            await service.Load("tom.schirgi@gmail.com");
            var modelState = new Mock<ModelStateDictionary>();
            await service.SetModelState(modelState.Object);

            List<PictureResponse> pics = await service.GetForAquarium("SchiScho2");

        }


        [Test]
        public async Task UploadImageAndDelete()
        {
            String unique = Guid.NewGuid().ToString();

            Aquarium aquarium = new Aquarium();
            aquarium.Depth = 65;
            aquarium.Height = 55;
            aquarium.Length = 150;
            aquarium.Name = unique;
            aquarium.WaterType = WaterType.Saltwater;

            UnitOfWork uow = new UnitOfWork();


            AquariumService service = new AquariumService(uow, uow.Aquariums, null);
            await service.Load("tom.schirgi@gmail.com");
            var modelState = new Mock<ModelStateDictionary>();
            await service.SetModelState(modelState.Object);

            ItemResponseModel<Aquarium> model = await service.CreateHandler(aquarium);

            Assert.NotNull(model);
            Assert.NotNull(model.Data);
            Assert.IsFalse(model.HasError);
            byte[] bytes = System.IO.File.ReadAllBytes("./TestImages/1.jpg");

            IFormFile file = new FormFile(new MemoryStream(bytes), 0, bytes.Length, "Data", "1.jpg");

            PictureRequest req = new PictureRequest();
            req.Description = "Ein Bild";
            req.FormFile = file;



            PictureService pservice = new PictureService(uow, uow.Pictures, null);
            await pservice.Load("tom.schirgi@gmail.com");
            ItemResponseModel<PictureResponse> resp = await pservice.AddPicture(unique, req);

            Assert.NotNull(resp);
            Assert.IsFalse(resp.HasError);


            ItemResultModel del = await pservice.Delete(unique, resp.Data.Picture.ID);

            Assert.IsTrue(del.Success);




        }
    }
}
