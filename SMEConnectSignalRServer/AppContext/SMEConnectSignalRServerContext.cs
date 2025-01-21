
using MongoDB.Driver;
using SMEConnectSignalRServer.Modals;

namespace SMEConnectSignalRServer.AppContext
{
    public class SMEConnectSignalRServerContext
    {
        private readonly IMongoDatabase _database;

        public SMEConnectSignalRServerContext(IConfiguration configuration) 
        {
            var connectionString = configuration.GetConnectionString("MongoDbConnection");

            var client = new MongoClient(connectionString);
            _database = client.GetDatabase("sample_mflix"); 
        }

        public IMongoCollection<Message> Messages => _database.GetCollection<Message>("messages");
    }

}
