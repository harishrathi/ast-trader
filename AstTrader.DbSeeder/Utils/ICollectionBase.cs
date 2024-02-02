using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AstTrader.DbSeeder.Utils
{
    public interface ICollectionBase
    {
        public string Id { get; set; }
    }

    public abstract class DbCollectionBase : ICollectionBase
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;
    }
}
