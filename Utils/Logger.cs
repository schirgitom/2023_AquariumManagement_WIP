using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public static class Logger
    {


        private static ILogger _Logger = null;

        private static Boolean IsInitialized = false;

        public static ILogger ContextLog<T>() where T : class
        {
            if (_Logger == null)
            {
                InitLogger();
            }

            return _Logger.ForContext<T>();
        }


        public static ILogger ILogger
        {
            get { return _Logger; }
        }

        /// <summary>Initializes the Logger based on the Logger Config file</summary>
        public static void InitLogger()
        {
            if (!IsInitialized)
            {
                String folder = Constants.CurrentFolder;

                Serilog.Debugging.SelfLog.Enable(message =>
                {
                    Console.WriteLine(message);
                });

                var configuration = new ConfigurationBuilder()
             .SetBasePath(folder)
             .AddJsonFile("appsettings.json")
             .Build();

                Log.Logger = new LoggerConfiguration()
                    .ReadFrom
                    .Configuration(configuration)
                    .CreateLogger();

                _Logger = Log.Logger;
                Log.Verbose("Logger initialized in Folder " + folder);
                IsInitialized = true;
            }

        }
    }
}
