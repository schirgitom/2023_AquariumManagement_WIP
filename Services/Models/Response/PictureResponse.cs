using DAL.Entities;

namespace Services.Models.Response
{
    public class PictureResponse
    {
        public byte[] Bytes { get; set; }

        public String Base64
        {
            get
            {
                if (Bytes != null)
                {
                    return Convert.ToBase64String(Bytes);
                }

                return "";
            }
        }

        public Picture Picture { get; set; }

    }
}
