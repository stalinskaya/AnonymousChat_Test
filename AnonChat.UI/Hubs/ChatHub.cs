using AnonChat.BLL.Interfaces;
using AnonChat.DAL.EF;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
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

    [Authorize]
    public class ChatHub : Hub
    {
        public readonly IAccountService accountService;
        public readonly IChatService chatService;
        

        public ChatHub (IAccountService accountService, IChatService chatService)
        {
            this.accountService = accountService;
            this.chatService = chatService;
        }

        public static List<UserIds> usersList = new List<UserIds>();

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
            await chatService.AddChatMessageAsync(caller.userId, message);
            await Clients.Client(caller.connId).SendAsync("Send Myself", message);
            if (receiver != null)
                await Clients.Client(receiver.connId).SendAsync("Send", message, caller.userId);
        }

        public async Task SendFaraway(string message, string receiverId)
        {
            UserIds receiver, caller;
            FindCallerReceiverByIds(receiverId, out caller, out receiver);
            bool chatRoomExist = chatService.ExistChat(caller.userId, receiverId);
            if (chatRoomExist)
            {
                await chatService.AddChatMessageAsync(caller.userId, message);
                await Clients.Client(receiver.connId).SendAsync("Send", message, caller.userId);
            }
            else
            {
                await chatService.AddChatAsync(receiverId, caller.userId);
                await chatService.AddChatMessageAsync(caller.userId, message);
                await Clients.Client(receiver.connId).SendAsync("Send", message, caller.userId);
            }
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
        public string GetConnectionId ()
        {
            return Context.ConnectionId;
        }

        void FindCallerReceiverByIds(string receiverId, out UserIds caller, out UserIds receiver)
        {
            receiver = usersList.Find(i => i.userId == receiverId);
            caller = usersList.Find(i => i.connId == Context.ConnectionId);
        }

        
    }
}
