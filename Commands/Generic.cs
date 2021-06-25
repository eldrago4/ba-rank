using Discord.Commands;
using MongoDB.Driver;
using MyCustomDiscordBot.Models;
using MyCustomDiscordBot.Services;
using System;
using System.Threading.Tasks;

namespace DiscordBot.Modules
{
    public class Generic : ModuleBase<SocketCommandContext>
    {
        private readonly DatabaseService _databaseService;

        public Generic(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        [Command("ping")]
        [Summary("Check whether the bot is working or not.")]
        public async Task Ping()
        {
            await ReplyAsync("Pong!");
        }

        [Command("register")]
        [Summary("Register your account for pick up games.")]
        public async Task Register()
        {
            try
            {
                //Collection in database regarding all users
                IMongoCollection<DbUser> collection = _databaseService.GetUserCollection();
                
                //Check if there exists a user in database by the Discord Id (of person who is calling command)
                var filter = Builders<DbUser>.Filter.Eq("DiscordId", Context.User.Id);
                long userCount = await collection.CountDocumentsAsync(filter);
                if (userCount > 0)
                {
                    //User exists
                    await ReplyAsync($"{Context.User.Mention}, you already have a registered account.");

                    //End command here
                    return;
                }

                //...Otherwise user does not exist yet in database.

                //Create a new DbUser at the local level, from the person who is calling the command
                DbUser dbUser = new DbUser(Context.User.Username, Context.User.Id);

                //Insert the user into database
                await collection.InsertOneAsync(dbUser);

                //Confirm
                await ReplyAsync($"{Context.User.Mention}, you have registered your account.");
            }
            catch(Exception e)
            {
                await ReplyAsync("Error registering your account: \n\n" + e.Message);
            }
            
        }
    }
}
