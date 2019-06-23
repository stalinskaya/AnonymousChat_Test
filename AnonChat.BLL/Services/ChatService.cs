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

        public bool ExistChat(string userid_1, string userid_2)
        {
            var chat1 = Database.UserChats.Find(i => i.UserId == userid_1);
            var chat2 = Database.UserChats.Find(i => i.UserId == userid_2);
            return chat1.Intersect(chat2).Any();
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

        public async Task AddChatMessageAsync(string userId,  string content)
        {
            var sender = await UserManager.FindByIdAsync(userId);
            var message = new ChatMessage
            {
                Sender = sender,
                Content = content,
                SendingTime = DateTime.Now
            };
            Database.ChatMessages.Create(message);
            Database.Save();
        }

        public async Task<List<Chat>> FindAllDialogs(string userId)
        {
            var user = await UserManager.FindByIdAsync(userId);
            var chats = Database.UserChats.Find(i => i.UserId == userId).Select(d=>d.Chat).ToList();
            return chats;
        }

        public ChatMessage AddChatMessage(string userId, string message, string chatId/*, IFormFileCollection files*/)
        {
            
            var newMessage = new ChatMessage
            {
                SenderId = userId,
                ChatId = chatId,
                Content = message,
                SendingTime = DateTime.Now
            };

            Database.ChatMessages.Create(newMessage);
            Database.Save();
            return newMessage;
        }
        
        public Chat GetDialog(string chatId)
        {
            try
            {
                var chat = Database.Chats.Find(i => i.ChatID == chatId).FirstOrDefault() ;

                return chat;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}