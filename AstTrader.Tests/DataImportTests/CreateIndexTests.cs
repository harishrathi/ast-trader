using System.Text.Json;

using AstTrader.DbSeeder.StockIndexDL;
using AstTrader.DbSeeder.Utils;
using AstTrader.Tests.TestUtils;

namespace AstTrader.Tests.DataImportTests
{
    [TestFixture]
    public class CreateIndexTests
    {
        [Test, Explicit]
        public void CreateIndexCollection()
        {
            var appSetting = AppSettingTest.GetAppSettings();
            var mongoDb = new MongoDbConnect(appSetting);
            var database = mongoDb.GetDatabase();

            var seeder = new StockIndexDbConfig(database);
            database.CreateCollection(StockIndexDbConfig.CollectionName);
            seeder.ConfigureCollection();
        }

        [Test, Explicit]
        public void CreateVNse500Index()
        {
            string jsonFilePath = "IndexLists/V40.json";
            File.Exists(jsonFilePath).Should().BeTrue();

            StockIndex index;
            using (StreamReader reader = File.OpenText(jsonFilePath))
            {
                string jsonString = reader.ReadToEnd();
                index = JsonSerializer.Deserialize<StockIndex>(jsonString) ?? throw new ArgumentNullException();
            }

            var appSetting = AppSettingTest.GetAppSettings();
            var mongoDb = new MongoDbConnect(appSetting);
            var database = mongoDb.GetDatabase();
            var stockIndexCollection = database.GetCollection<StockIndex>(StockIndexDbConfig.CollectionName);
            stockIndexCollection.InsertOne(index);
        }
    }
}
