using AnonChat.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AnonChat.BLL.Interfaces
{
    public interface IChatService
    {
        Task<bool> ExistChat(string userId, string receiverId);
        Task<List<Chat>> FindAllDialogs(string userId);
        Chat AddChat(string receiverId, string senderId);
        ChatMessage AddChatMessage(string userId, string message, string chatId);
        Task<Chat> GetDialog(string userId, string companionId);
    }
}
