using Microsoft.Extensions.Configuration;

namespace Utils
{
    public class SettingsReader
    {
        public SettingsReader()
        {
        }

        public T GetSettings<T>(String section) where T : class
        {
            var builder = new ConfigurationBuilder().SetBasePath(Constants.CurrentFolder).AddJsonFile("Settings/appsettings.json");

            return builder.Build().GetSection(section).Get<T>();
        }
    }
}
