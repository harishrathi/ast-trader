using MongoDB.Bson;
using MongoDB.Driver;

namespace AstTrader.DbSeeder.Utils
{
    public class MongoDbConnect
    {
        private readonly ApplicationSettings appSettings;

        public MongoDbConnect(ApplicationSettings settings)
        {
            appSettings = settings;
        }

        public void CheckMongDbConnection()
        {
            // Send a ping to confirm a successful connection
            try
            {
                var client = GetClient();
                var result = client.GetDatabase("admin").RunCommand<BsonDocument>(new BsonDocument("ping", 1));
                Console.WriteLine("Pinged your deployment. You successfully connected to MongoDB!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public IMongoDatabase GetDatabase()
        {
            var client = GetClient();
            return client.GetDatabase(appSettings.ConnectionStrings.MongoDbName);
        }

        private MongoClient GetClient()
        {
            var settings = MongoClientSettings.FromConnectionString(appSettings.ConnectionStrings.MongoDb);
            settings.ServerApi = new ServerApi(ServerApiVersion.V1);
            return new MongoClient(settings);
        }
    }
}
