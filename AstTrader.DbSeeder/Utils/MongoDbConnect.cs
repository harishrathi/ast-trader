using MongoDB.Bson;
using MongoDB.Driver;

namespace AstTrader.DbSeeder.Utils
{
    public class MongoDbConnect
    {
        private readonly AppSettings appSettings;

        public MongoDbConnect(AppSettings settings)
        {
            appSettings = settings;
        }

        public void CheckMongDbConnection()
        {
            // Send a ping to confirm a successful connection
            try
            {
                var settings = MongoClientSettings.FromConnectionString(appSettings.ConnectionStrings.MongoDbConnString);
                var client = new MongoClient(settings);
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
            var settings = MongoClientSettings.FromConnectionString(appSettings.ConnectionStrings.MongoDbConnString);
            var client = new MongoClient(settings);
            return client.GetDatabase(appSettings.ConnectionStrings.MongoDbConnString);
        }
    }
}
