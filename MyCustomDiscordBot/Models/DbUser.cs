using MongoDB.Bson;
using System.Collections.Generic;

namespace MyCustomDiscordBot.Models
{
    public class MapRecord
    {
        public string MapName { get; }
        public int Wins { get; set; }
        public int Losses { get; set; }

        public MapRecord(string mapName)
        {
            MapName = mapName;
            Wins = 0;
            Losses = 0;
        }
    }

    public class DbUser
    {
        public ObjectId Id { get; set; }
        public string Username { get; set; }
        public ulong DiscordId { get; set; }
        public int ELO { get; set; }
        public List<MapRecord> MapRecords { get; set; }

        public DbUser(string username, ulong discordId)
        {
            Id = ObjectId.GenerateNewId();
            DiscordId = discordId;
            Username = username;
            DiscordId = discordId;
            ELO = 0;
            MapRecords = new List<MapRecord>();
        }
    }
}