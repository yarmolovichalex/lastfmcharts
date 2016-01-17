using System.Configuration;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DB
{
    public class DBService : IDBService
    {
        protected static IMongoClient client;
        protected static IMongoDatabase database;

        public DBService()
        {
            var connectionString = ConfigurationManager.AppSettings.Get("MONGOLAB_URI") ?? "mongodb://localhost:27017/lastfmcharts";

            client = new MongoClient(connectionString);
            database = client.GetDatabase(MongoUrl.Create(connectionString).DatabaseName);
        }

        public int GetArtistViews(string name)
        {
            var collection = database.GetCollection<BsonDocument>("popularity");
            var filter = Builders<BsonDocument>.Filter.Eq("name", name);
            var result = collection.Find(filter).ToList().FirstOrDefault();

            if (result != null)
            {
                var oldViews = (int) result["views"];
                var newViews = oldViews + 1;
                var update = Builders<BsonDocument>.Update.Set("views", newViews);
                collection.UpdateOne(filter, update);

                return oldViews;
            }
            else
            {
                var document = new BsonDocument
                {
                    { "name", name },
                    { "views", 1 }
                };

                collection.InsertOne(document);

                return 0;
            }
        }
    }
}
