using System;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AnonChat.Models;
using AnonChat.BLL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using AnonChat.DAL.Interfaces;
using System.Linq;

namespace AnonChat.BLL.Services
{
    public class ChatService : IChatService
    {
        IUnitOfWork Database { get; set; }
        private UserManager<ApplicationUser> UserManager;

        public ChatService(IUnitOfWork Database, UserManager<ApplicationUser> UserManager) {
            this.Database = Database;
            this.UserManager = UserManager;
        }

        public async Task AddChatAsync(string userid_1, string userid_2)
        {
            var user1 = await UserManager.FindByIdAsync(userid_1);
            var user2 = await UserManager.FindByIdAsync(userid_2);
            var chat = new Chat
            {
                ChatID = "Anonymous chat:" + DateTimeOffset.Now,
                StatusAnonymity = true,
                StatusBlock = false
            };
            Database.Chats.Create(chat);
            Database.Save();

            var userChat1 = new UserChat
            {
                User = user1,
                Chat = chat
            };
            Database.UserChats.Create(userChat1);
            Database.Save();

            var userChat2 = new UserChat
            {
                User = user2,
                Chat = chat
            };
            Database.UserChats.Create(userChat2);
            Database.Save();
        }

        public async Task AddMessageAsync(string userId, string receiverId, string content)
        {
            var receiver = await UserManager.FindByIdAsync(receiverId);
            var sender = await UserManager.FindByIdAsync(userId);
            var message = new ChatMessage
            {
                Receiver = receiver,
                Sender = sender,
                Content = content,
                SendingTime = DateTime.Now,

            };
            Database.ChatMessages.Create(message);
            Database.Save();
        }

        //public IEnumerable<ChatMessage> GetChatMessages(string senderId, string recieverId)
        //{
        //    var query = Database.ChatMessages.Find(c => c.SenderId == senderId && c.ReceiverId == recieverId);
        //    query = query.OrderBy(m => m.SendingTime);
        //    return (query.ToList());
        //}
    }
}