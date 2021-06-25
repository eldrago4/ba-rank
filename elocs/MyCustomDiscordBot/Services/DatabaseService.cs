using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MyCustomDiscordBot.Models;
using MyCustomDiscordBot.Settings;
using System;

namespace MyCustomDiscordBot.Services
{
    public class DatabaseService
    {
        public static MongoClient Client { get; set; }
        public static IMongoDatabase Database { get; set;  }

        public readonly BotSettings _botSettings;

        public DatabaseService(IOptions<BotSettings> botSettings)
        {
            _botSettings = botSettings.Value;
            Client = new MongoClient(_botSettings.DBConnectionString);
            Database = Client.GetDatabase("myDatabase");
        }

        public IMongoCollection<DbUser> GetUserCollection()
        {
            return Database.GetCollection<DbUser>("Users");
        }
    }
}
