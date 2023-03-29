using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models.Request
{
    public class PictureRequest
    {
        public String Description { get; set; }

        public IFormFile FormFile { get; set; }
    }
}
