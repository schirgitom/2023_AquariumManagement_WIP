using DAL.Entities;
using DAL.Repository.Impl;
using DAL.UnitOfWork;
using MimeKit;
using MongoDB.Bson;
using Services.Models.Request;
using Services.Models.Response;
using Services.Models.Response.Basis;

namespace Services
{
    public class PictureService : Service<Picture>
    {
        public PictureService(UnitOfWork uow, IPictureRepository repo, GlobalService service) : base(uow, repo, service)
        {
        }

        public override async Task<bool> Validate(Picture entry)
        {
            if (entry != null)
            {

            }
            else
            {
                validationDictionary.AddError("NotValid", "Item is no Coral");
            }

            return validationDictionary.IsValid;
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
                    await Repository.DeleteByIdAsync(picture);

                    result.Success = true;
                }
                catch (Exception ex)
                {
                    log.Warning(ex, "Delete Failed");
                    result.Success = false;
                    result.HasError = true;
                    result.ErrorMessages.Add("Failed", "Delete Failed");
                }
            }
            else
            {
                result.Success = false;
                result.HasError = true;
                result.ErrorMessages.Add("NotFound", "Image not found");
            }

            return result;

        }

        public async Task<ItemResponseModel<PictureResponse>> AddPicture(String aquarium, PictureRequest pict)
        {

            ItemResponseModel<PictureResponse> ret = new ItemResponseModel<PictureResponse>();
            ret.Data = null;
            ret.HasError = true;
            if (pict.FormFile != null)
            {

                String filename = pict.FormFile.FileName;
                try
                {
                    if (!String.IsNullOrEmpty(filename))
                    {
                        String type = MimeTypes.GetMimeType(pict.FormFile.FileName);


                        if (type.StartsWith("image/"))
                        {
                            byte[] bin = null;
                            using (var stream = new MemoryStream())
                            {
                                pict.FormFile.CopyTo(stream);
                                bin = stream.ToArray();
                            }

                            ObjectId id = await UnitOfWork.Context.GridFSBucket.UploadFromBytesAsync(pict.FormFile.FileName, bin);
                            Picture pic = new Picture();
                            pic.PictureID = id.ToString();
                            pic.Uploaded = DateTime.Now;
                            pic.Aquarium = aquarium;
                            pic.ContentType = type;

                            if (pict != null)
                            {
                                pic.Description = pict.Description;
                            }

                            Picture fpic = await UnitOfWork.Pictures.InsertOneAsync(pic);

                            var bytes = await UnitOfWork.Context.GridFSBucket.DownloadAsBytesAsync(id);

                            PictureResponse response = new PictureResponse();
                            response.Picture = fpic;
                            response.Bytes = bytes;
                            ret.HasError = false;
                            ret.Data = response;

                        }
                        else
                        {
                            ret.ErrorMessages.Add("FileContent", "Uploaded File is no image");
                        }

                    }
                    else
                    {
                        ret.ErrorMessages.Add("FileName", "Filename is empty");
                    }


                }
                catch (Exception e)
                {
                    log.Warning(e, "Upload image failed");
                    ret.ErrorMessages.Add("Error", "Error during uploading");
                }
            }
            else
            {
                ret.ErrorMessages.Add("FileName", "File is empty");
            }
            return ret;
        }

        public async Task<List<PictureResponse>> GetForAquarium(String aquarium)
        {
            List<PictureResponse> respo = new List<PictureResponse>();
            List<Picture> pictures = await Repository.FilterByAsync(x => x.Aquarium.Equals(aquarium));

            foreach (Picture pi in pictures)
            {

                var bytes = await UnitOfWork.Context.GridFSBucket.DownloadAsBytesAsync(new ObjectId(pi.PictureID));

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
            Picture pictures = await Repository.FindByIdAsync(id);

            var bytes = await UnitOfWork.Context.GridFSBucket.DownloadAsBytesAsync(new ObjectId(pictures.PictureID));

            PictureResponse response = new PictureResponse();
            response.Picture = pictures;
            response.Bytes = bytes;

            ret.Data = response;
            return ret;
        }


        protected override Task<ItemResponseModel<Picture>> Create(Picture entry)
        {
            throw new NotImplementedException();
        }

        public override Task<ItemResponseModel<Picture>> Update(string id, Picture entry)
        {
            throw new NotImplementedException();
        }
    }
}
