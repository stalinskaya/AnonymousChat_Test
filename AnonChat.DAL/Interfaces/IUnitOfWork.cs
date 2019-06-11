using AnonChat.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AnonChat.DAL.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<ChatMessage> ChatMessages { get; }
        IRepository<ApplicationUser> Users { get; }
        IRepository<LogDetail> LogDetails { get; }

        Task SaveAsync();
        void Save();
    }
}
