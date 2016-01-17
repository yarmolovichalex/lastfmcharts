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
            client = new MongoClient(ConfigurationManager.AppSettings["MONGOLAB_URI"]);
            database = client.GetDatabase("lastfmcharts");
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
