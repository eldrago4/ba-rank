using Microsoft.Extensions.Options;
using MyCustomDiscordBot.Models;
using MyCustomDiscordBot.Settings;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyCustomDiscordBot.Services
{
    public class Queue
    {
        public int Capacity { get; set; }
        public List<DbUser> Users { get; set; }

        public Queue(int capacity)
        {
            Capacity = capacity;
            Users = new List<DbUser>();
        }

        /// <summary>
        /// Always returns a string with success or error message.
        /// </summary>
        /// <param name="dbUser"></param>
        /// <returns></returns>
        public string Add(DbUser dbUser)
        {
            //Uncomment for production - stops multiple joining in queue
            /*
            foreach(DbUser userCurr in Users)
            {
                if(userCurr.DiscordId == dbUser.DiscordId)
                {
                    return "You are already in the queue.";
                }
            }
            */

            Users.Add(dbUser);

            //check if queue is full
            if (Users.Count >= Capacity)
            {
                string returnVal = "The queue is full. Match starting...";

                Users.Clear();

                return returnVal;
            }

            //add user
            
            return $"You have been added to the queue. `{Users.Count} / {Capacity}.`";
        }

        /// <summary>
        /// Always returns a string with success or error message.
        /// </summary>
        /// <param name="discordId"></param>
        /// <returns></returns>
        public string Remove(ulong discordId)
        {
            //Look for the index to remove the user from the list
            int removeIndex = -1;
            for(int i = 0; i < Users.Count; i++)
            {
                if(Users[i].DiscordId == discordId)
                {
                    //save the position of the user to remove
                    removeIndex = i;
                    break;
                }
            }

            if(removeIndex >= 0)
            {
                //a user has been found in the queue by parameter: discordId
                string returnVal = $"You have been removed from the queue. `{Users.Count - 1} / {Capacity}.`";
                Users.RemoveAt(removeIndex);
                return returnVal;
            }

            //No user was found
            return "You were not in the queue.";
        }
    }

    public class QueueService
    {
        private readonly GameSettings _gameSettings;
        public Queue NAQueue { get; set; }

        public QueueService(IOptions<GameSettings> gameSettings)
        {
            _gameSettings = gameSettings.Value;

            NAQueue = new Queue(_gameSettings.TeamSize * 2);
        }
    }
}
