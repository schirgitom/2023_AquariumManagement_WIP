using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public class SettingsReader
    {
        public SettingsReader() { }

        public T GetSettings<T>(String section) where T : class
        {
            var builder = new ConfigurationBuilder().SetBasePath(Constants.CurrentFolder)
                .AddJsonFile("appsettings.json");

            return builder.Build().GetSection(section).Get<T>();
        }
   
    }
}
