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

namespace AnonChat.BLL.Services
{
    public class ChatService : IChatService
    {
        IUnitOfWork Database { get; set; }
        private UserManager<ApplicationUser> UserManager;

        public void AddMessage (ChatMessage message)
        {
            Database.ChatMessages.Create(message);
            Database.Save();
          }

        
    }
}
