using DAL;
using DAL.Entities;
using DAL.Repository;
using MimeKit;
using MongoDB.Bson;
using Services.Models.Request;
using Services.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class PictureService : Service<Picture>
    {
        public PictureService(UnitOfWork uow, IRepository<Picture> repository, GlobalService service) : base(uow, repository, service)
        {
        }

        public override Task<ItemResponseModel<Picture>> Update(string id, Picture entity)
        {
            throw new NotImplementedException();
        }

        public override async Task<bool> Validate(Picture entity)
        {
            if(entity != null)
            {

            }
          else
            {
                modelStateWrapper.AddError("Empty", "Picture is empty");
            }

            return modelStateWrapper.IsValid;
        }

        protected override Task<ItemResponseModel<Picture>> Create(Picture entity)
        {
            throw new NotImplementedException();
        }

        public async Task<ItemResponseModel<PictureResponse>> AddPicture(String aquarium, PictureRequest request)
        {
            ItemResponseModel<PictureResponse> returnmodel = new ItemResponseModel<PictureResponse>();
            returnmodel.Data = null;
            returnmodel.HasError = true;

            if(request.FormFile != null)
            {
                String filename = request.FormFile.FileName;

                if(!String.IsNullOrEmpty(filename))
                {
                    String typ = MimeTypes.GetMimeType(filename);

                    if(typ.StartsWith("image/"))
                    {
                        byte[] binaries = null;

                        using(var stream = new MemoryStream())
                        {
                            request.FormFile.CopyTo(stream);
                            binaries = stream.ToArray();
                        }

                        ObjectId pictureid = await UnitOfWork.Context.GridFSBucket.UploadFromBytesAsync(filename, binaries);

                        Picture pic = new Picture();
                        pic.PictureID = pictureid.ToString();
                        pic.Description = request.Description;
                        pic.Aquarium = aquarium;
                        pic.ContentType = typ;

                        Picture indb = await UnitOfWork.Pictures.InsertOneAsync(pic);

                        var bytes = await UnitOfWork.Context.GridFSBucket.DownloadAsBytesAsync(pictureid);

                        PictureResponse response = new PictureResponse();
                        response.Picture = indb;
                        response.Bytes = bytes;
                        returnmodel.Data = response;
                        returnmodel.HasError = false;

                    }
                    else
                    {
                        returnmodel.ErrorMessages.Add("Only images are allowed");
                    }
                }
                else
                {
                    returnmodel.ErrorMessages.Add("Filename is empty");
                }
            }
            else
            {
                returnmodel.ErrorMessages.Add("No Picture provided");
            }


            return returnmodel;
        }

        public async Task<ItemResultModel> Delete(String aquarium, String picture)
        {
            ItemResultModel result = new ItemResultModel();

            Picture pic = await UnitOfWork.Pictures.FindByIdAsync(picture);

            if (pic != null)
            {
                try
                {
                    await UnitOfWork.Context.GridFSBucket.DeleteAsync(new ObjectId(pic.PictureID));
                    await repository.DeleteByIdAsync(picture);

                    result.Success = true;
                }
                catch (Exception ex)
                {
                    log.Warning(ex, "Delete Failed");
                    result.Success = false;
                    result.HasError = true;
                    result.ErrorMessages.Add("Delete Failed");
                }
            }
            else
            {
                result.Success = false;
                result.HasError = true;
                result.ErrorMessages.Add("Image not found");
            }

            return result;

        }

        

        public async Task<List<PictureResponse>> GetForAquarium(String aquarium)
        {
            List<PictureResponse> respo = new List<PictureResponse>();
            List<Picture> pictures = await repository.FilterByAsync(x => x.Aquarium.Equals(aquarium));

            foreach (Picture pi in pictures)
            {

                var bytes = await UnitOfWork.Context.GridFSBucket.DownloadAsBytesAsync(pi.PictureID);

                PictureResponse response = new PictureResponse();
                response.Picture = pi;
                response.Bytes = bytes;

                respo.Add(response);
            }

            return respo;
        }

        public async Task<ItemResponseModel<PictureResponse>> GetPicture(String id)
        {
            ItemResponseModel<PictureResponse> ret = new ItemResponseModel<PictureResponse>();
            Picture pictures = await repository.FindByIdAsync(id);

            var bytes = await UnitOfWork.Context.GridFSBucket.DownloadAsBytesAsync(pictures.PictureID);

            PictureResponse response = new PictureResponse();
            response.Picture = pictures;
            response.Bytes = bytes;

            ret.Data = response;
            return ret;
        }



    }
}
