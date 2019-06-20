using AnonChat.BLL.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnonChat.UI.Hubs
{
    public interface IChatHub
    {
        string GetConnectionId();
        Task Send(string message, string receiverId);
    }
}
