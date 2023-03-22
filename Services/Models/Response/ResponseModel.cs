using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models.Response
{
    public class ResponseModel
    {
        public Boolean HasError { get; set; }

        public List<String> ErrorMessages { get; set; } = new();


    }
}
