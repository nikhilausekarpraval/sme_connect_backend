using DemoDotNetCoreApplication.Modals;
using MongoDB.Driver;

namespace DemoDotNetCoreApplication.Data
{
    public class SMEConnectSignalRServerContext
    {
        private readonly IMongoDatabase _database;

        public SMEConnectSignalRServerContext(string connectionString)
        {
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase("smeconnect.8vs6a.mongodb.net"); 
        }

        public IMongoCollection<Message> Messages => _database.GetCollection<Message>("messages");
    }
}
