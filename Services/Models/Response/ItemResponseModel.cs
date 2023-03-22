using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models.Response
{
    public class ItemResponseModel<T> : ResponseModel where T : class
    {
        public T Data { get; set; }
    }
}
