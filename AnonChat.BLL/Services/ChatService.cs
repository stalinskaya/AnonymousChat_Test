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
        private IAccountService accountService { get; set; }

        public ChatService(IUnitOfWork Database, IAccountService accountService) {
            this.Database = Database;
            this.accountService = accountService;
        }

        public async Task<bool> ExistChat(string userId, string receiverId)
        {
            var user = await accountService.FindUserById(userId);

            var incomingDialogs = user.ReceivedChats.Where(x =>
                x.ReceiverId == userId && x.SenderId == receiverId)
                .ToList();

            var outgoingDialogs = user.SentChats.Where(x =>
                x.ReceiverId == receiverId && x.SenderId == userId)
                .ToList();

            var fullList = new List<Chat>();
            fullList.AddRange(incomingDialogs);
            fullList.AddRange(outgoingDialogs);

            return fullList.Any();
        }

        public Chat AddChat (string receiverId, string senderId)
        {
            var newDialog = new Chat
            {
                ChatID = "AnonymousChat: " + DateTimeOffset.Now,
                SenderId = senderId,
                ReceiverId = receiverId,
                StatusAnonymity = true,
                StatusBlock = false
            };

            Database.Chats.Create(newDialog);
            Database.Save();

            return newDialog;
        }
        public ChatMessage AddChatMessageAsync(string userId, string message, string dialogId)
        {
            try
            {
                var newMessage = new ChatMessage
                {
                    SenderId = userId,
                    ChatId = dialogId,
                    Content = message,
                    SendingTime = DateTime.Now
                };

               Database.ChatMessages.Create(newMessage);
               Database.Save();
               return newMessage;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<List<Chat>> FindAllDialogs(string userId)
        {
            var user = await accountService.FindUserById(userId);

            var incomingDialogs = user.ReceivedChats.Where(x =>
                x.ReceiverId == userId).ToList();

            var outgoingDialogs = user.SentChats.Where(x =>
                x.SenderId == userId).ToList();

            var chats = new List<Chat>();
            chats.AddRange(incomingDialogs);
            chats.AddRange(outgoingDialogs);

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
        
        public async Task<Chat> GetDialog(string userId, string companionId)
        {
            var user = await accountService.FindUserById(userId);

            var incomingDialogs = user.ReceivedChats.Where(x =>
                    x.ReceiverId == userId && x.SenderId == companionId)
                .ToList();

            var outgoingDialogs = user.SentChats.Where(x =>
                    x.ReceiverId == companionId && x.SenderId == userId)
                .ToList();

            var fullList = new List<Chat>();
            fullList.AddRange(incomingDialogs);
            fullList.AddRange(outgoingDialogs);

            var result = fullList.First();

            return result;
        }
    }
}