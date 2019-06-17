using AnonChat.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AnonChat.BLL.Interfaces
{
    public interface IChatService
    {
        void AddChat(Chat chat);
        bool DoesChatExist(string senderId);
        void AddMessage(ChatMessage message);
        IEnumerable<ChatMessage> GetChatMessages(string senderId, string recieverId);

    }
}
