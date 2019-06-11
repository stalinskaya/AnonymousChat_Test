using AnonChat.BLL.Interfaces;
using AnonChat.DAL.EF;
using AnonChat.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnonChat.BLL.Hubs
{
    public class ChatHub : Hub
    {
        public readonly IAccountService accountService;
        public Task Send(ChatMessage message)
        {
            string SenderId = message.SenderId.ToString();
            var sender = accountService.FindUserById(SenderId).Result;
            message.Sender = sender;
            return Clients.User(sender.Id).SendAsync("ReceiveMessage", message);
        }
    }
}
