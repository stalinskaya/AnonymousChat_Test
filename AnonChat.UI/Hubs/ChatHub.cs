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
    public class UserIds
    {
        public string userId { get; set; }
        public string connId { get; set; }
    }
    public class ChatHub : Hub
    {
        public readonly IAccountService accountService;
        public readonly IChatService chatService;

        public ChatHub (IAccountService accountService, IChatService chatService)
        {
            this.accountService = accountService;
            this.chatService = chatService;
        }

        static List<UserIds> usersList = new List<UserIds>();

        public async override Task OnConnectedAsync()
        {
            var callerId = await accountService.FindIdByEmail(Context.User.Identity.Name);
            UpdateList(callerId);
            await base.OnConnectedAsync();
        }

        public async Task Send(string message, string receiverId)
        {
            UserIds receiver, caller;
            FindCallerReceiverByIds(receiverId, out caller, out receiver);
            await chatService.AddMessageAsync(caller.userId, receiverId, message);
            await Clients.Client(caller.connId).SendAsync("Send Myself", message);
            if (receiver != null)
                await Clients.Client(receiver.connId).SendAsync("Send", message, caller.userId);
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            usersList.Remove(usersList.Find(u => u.connId == Context.ConnectionId));
            return base.OnDisconnectedAsync(exception);
        }

        void UpdateList (string callerId)
        {
            var index = usersList.FindIndex(i => i.userId == callerId);
            if (index != -1 && usersList[index].connId != Context.ConnectionId)
            {
                usersList[index].connId = Context.ConnectionId;
            }
            else {
                usersList.Add(new UserIds { connId = Context.ConnectionId, userId = callerId });
            }
        }

        void FindCallerReceiverByIds(string receiverId, out UserIds caller, out UserIds receiver)
        {
            receiver = usersList.Find(i => i.userId == receiverId);
            caller = usersList.Find(i => i.connId == Context.ConnectionId);
        }
    }
}
