using MongoDB.Bson;
using MongoDB.Driver;
using TradeAstra.Core.Config;

namespace AstTrader.DbSeeder.Utils
{
    public class MongoDbConnect
    {
        private readonly AppConnectionString appConnectionString;
        public MongoDbConnect(AppConnectionString appConnectionString)
        {
            this.appConnectionString = appConnectionString;
        }

        public void CheckMongDbConnection()
        {
            // Send a ping to confirm a successful connection
            try
            {
                var settings = MongoClientSettings.FromConnectionString(appConnectionString.MongoDbConnString);
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
            var settings = MongoClientSettings.FromConnectionString(appConnectionString.MongoDbConnString);
            var client = new MongoClient(settings);
            return client.GetDatabase(appConnectionString.MongoDbConnString);
        }
    }
}
