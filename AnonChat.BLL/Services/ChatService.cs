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
        //private UserManager<ApplicationUser> UserManager;

        public void AddChat(Chat chat)
        {
            Database.Chats.Create(chat);
            Database.Save();
        }

        public bool DoesChatExist(string senderId)
        {
            Database.Chats.Find(c => c.ChatID == senderId);
            return true;
        }


        public void AddMessage(ChatMessage message)
        {
            Database.ChatMessages.Create(message);
            Database.Save();
        }

        public IEnumerable<ChatMessage> GetChatMessages(string senderId, string recieverId)
        {
            var query = Database.ChatMessages.Find(c => c.SenderId == senderId && c.ReceiverId == recieverId);
            query = query.OrderBy(m => m.SendingTime);
            return (query.ToList());
        }
    }
}