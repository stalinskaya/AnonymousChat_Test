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
        Task AddMessageAsync(string userId, string receiverId, string content);

    }
}
