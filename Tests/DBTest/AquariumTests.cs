using DAL;
using DAL.Entities;
using DAL.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Services;
using Services.Models.Request;
using Services.Models.Response;
using Services.Models.Response.Basis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.DBTest
{
    public class AquariumTests
    {
        [Test]
        public async Task CreateAquarium()
        {
            Aquarium aquarium = new Aquarium();
            aquarium.Liters = 500;
            aquarium.Height = 55;
            aquarium.Depth = 65;
            aquarium.Length = 150;
            aquarium.WaterType = WaterType.Saltwater;
            aquarium.Name = "SchiScho";

            UnitOfWork uow = new UnitOfWork();

            Aquarium fromdb = await uow.Aquariums.InsertOneAsync(aquarium);

            Assert.NotNull(fromdb);
            Assert.NotNull(fromdb.ID);


        }

        [Test]
        public async Task UploadImage()
        {
            UnitOfWork uow = new UnitOfWork();
            PictureService service = new PictureService(uow, uow.Pictures, null);

            PictureRequest request = new PictureRequest();
            request.Description = "So unglaublich flauschig. ";

            byte[] bytes = System.IO.File.ReadAllBytes(@"C:\Users\SchirgiT\OneDrive\image.jpg");

            IFormFile file = new FormFile(new MemoryStream(bytes), 0, bytes.Length, "Data", "image.jpg");

            request.FormFile = file;

            List<Picture> pictures = await uow.Pictures.FilterByAsync(x => true);
            Assert.Greater(pictures.Count, 0);
            int old = pictures.Count;

            await service.AddPicture("SchiScho", request);

            pictures = await uow.Pictures.FilterByAsync(x => true);
            Assert.AreEqual(pictures.Count, old +1);

        }



        [Test]
        public async Task UploadImage2()
        {
            UnitOfWork uow = new UnitOfWork();
            PictureService service = new PictureService(uow, uow.Pictures, null);

            PictureRequest request = new PictureRequest();
            request.Description = "So unglaublich flauschig. ";

            byte[] bytes = System.IO.File.ReadAllBytes(@"C:\Users\SchirgiT\OneDrive\image.jpg");

            IFormFile file = new FormFile(new MemoryStream(bytes), 0, bytes.Length, "Data", "image.jpg");

            request.FormFile = file;

            List<Picture> pictures = await uow.Pictures.FilterByAsync(x => true);
            Assert.Greater(pictures.Count, 0);
            int old = pictures.Count;

            ItemResponseModel<PictureResponse> pics = await service.AddPicture("SchiScho", request);

            pictures = await uow.Pictures.FilterByAsync(x => true);
            Assert.AreEqual(pictures.Count, old + 1);
            Assert.IsFalse(pics.HasError);

        }
    }
}
