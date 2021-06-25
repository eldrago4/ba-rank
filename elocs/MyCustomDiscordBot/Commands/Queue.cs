using Discord.Commands;
using MongoDB.Driver;
using MyCustomDiscordBot.Models;
using MyCustomDiscordBot.Services;
using System;
using System.Threading.Tasks;

namespace DiscordBot.Modules
{
    public class Queue : ModuleBase<SocketCommandContext>
    {
        private readonly QueueService _queueService;
        private readonly DatabaseService _databaseService;

        public Queue(QueueService queueService, DatabaseService databaseService)
        {
            _queueService = queueService;
            _databaseService = databaseService;
        }

        [Command("join")]
        [Alias(new string[] { "j" })]
        [Summary("Join the queue for pick up games.")]
        public async Task JoinNAQueue()
        {
            IMongoCollection<DbUser> collection = _databaseService.GetUserCollection();

            //check if user is registered
            DbUser dbUser = await collection.Find(x => x.DiscordId == Context.User.Id).FirstOrDefaultAsync();
            if(dbUser == null)
            {
                await ReplyAsync($"{Context.User.Mention} : You are not registered in our database. Please register first.");
                return;
            }

            //Let's add them to the queue
            string reply = _queueService.NAQueue.Add(dbUser);
            await ReplyAsync($"{Context.User.Mention} : {reply}");
        }

        [Command("leave")]
        [Alias(new string[] { "l" })]
        [Summary("Leave the pick up games queue.")]
        public async Task LeaveNAQueue()
        {
            //No need to check for registering. Just simply give them the reply.
            string reply = _queueService.NAQueue.Remove(Context.User.Id);
            await ReplyAsync($"{Context.User.Mention} : {reply}");
        }

        [Command("queue")]
        [Alias(new string[] { "q" })]
        [Summary("See how many people are in the queue.")]
        public async Task SeeNAQueue()
        {
            await ReplyAsync($"There are `{_queueService.NAQueue.Users.Count} / {_queueService.NAQueue.Capacity}` users in the queue.");
        }
    }
}
