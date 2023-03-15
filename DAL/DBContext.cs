using MongoDB.Driver.GridFS;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using Utils;

namespace DAL
{
    public class DBContext
    {
        ILogger log = Logger.ContextLog<DBContext>();

        public IMongoDatabase DataBase { get; set; }
        public GridFSBucket GridFSBucket { get; private set; }
        MongoClient Client;
        public bool IsConnected
        {
            get
            {
                return DataBase != null;
            }
        }

        public DBContext()
        {

            log.Debug("Connecting to Database");


            Task tks = Connect();
            tks.Wait();

        }

        public async Task Connect()
        {
            SettingsReader reader = new SettingsReader();
            DBSettings settings = reader.GetSettings<DBSettings>("MongoDbSettings");


            MongoClientSettings clientsettings = new MongoClientSettings();
            clientsettings.Server = new MongoServerAddress(settings.Server, settings.Port);

            if (!string.IsNullOrEmpty(settings.Username) && !string.IsNullOrEmpty(settings.Password))
            {
                clientsettings.Credential = MongoCredential.CreateCredential("admin", settings.Username, settings.Password);
            }


            Client = new MongoClient(clientsettings);
            DataBase = Client.GetDatabase(settings.DatabaseName);

            GridFSBucket = new GridFSBucket(DataBase);

            //  https://mongodb.github.io/mongo-csharp-driver/2.13/reference/gridfs/gettingstarted/

            if (DataBase != null)
            {
                log.Information("Successfully connected to Mongo DB " + settings.Server + ":" + settings.Port);
            }
            else
            {

                log.Fatal("Could not connect to Mongo DB " + settings.Server + ":" + settings.Port);
            }
        }

        public async Task Disconnect()
        {

        }


    }
}
