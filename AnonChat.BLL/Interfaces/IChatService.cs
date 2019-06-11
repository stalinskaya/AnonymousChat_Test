using AnonChat.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AnonChat.BLL.Interfaces
{
    public interface IChatService
    {
        void AddMessage(ChatMessage chatMessage);
    }
}
