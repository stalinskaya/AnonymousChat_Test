using AnonChat.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AnonChat.BLL.Interfaces
{
    public interface IChatService
    {
        Task AddChatAsync(string userid_1, string userid_2);
        bool ExistChat(string userid_1, string userid_2);
        Task AddChatMessageAsync(string userId, string content);
        Task<List<Chat>> FindAllDialogs(string userId);
        ChatMessage AddChatMessage(string userId, string message, string chatId);
        Chat GetDialog(string chatId);
    }
}
