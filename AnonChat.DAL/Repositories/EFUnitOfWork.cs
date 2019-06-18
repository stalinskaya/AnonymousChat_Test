using AnonChat.DAL.EF;
using AnonChat.DAL.Interfaces;
using AnonChat.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AnonChat.DAL.Repositories
{
    public class EFUnitOfWork : IUnitOfWork
    {
        private ChatContext db;

        private ApplicationUserRepository applicationUserRepository;
        private LogDetailRepository logDetailRepository;
        private ChatMessageRepository chatMessageRepository;
        private FileRepository fileRepository;
        private ChatRepository chatRepository;
        private UserChatRepository userChatRepository;

        public EFUnitOfWork(ChatContext db)
        {
            this.db = db;
        }


        public IRepository<ApplicationUser> Users
        {
            get
            {
                if (applicationUserRepository == null)
                    applicationUserRepository = new ApplicationUserRepository(db);
                return applicationUserRepository;
            }
        }

        public IRepository<LogDetail> LogDetails
        {
            get
            {
                if (logDetailRepository == null)
                    logDetailRepository = new LogDetailRepository(db);
                return logDetailRepository;
            }
        }
        public IRepository<ChatMessage> ChatMessages
        {
            get
            {
                if (chatMessageRepository == null)
                    chatMessageRepository = new ChatMessageRepository(db);
                return chatMessageRepository;
            }
        }

        public IRepository<Chat> Chats
        {
            get
            {
                if (chatRepository == null)
                    chatRepository = new ChatRepository(db);
                return chatRepository;
            }
        }

        public IRepository<UserChat> UserChats
        {
            get
            {
                if (userChatRepository == null)
                    userChatRepository = new UserChatRepository(db);
                return userChatRepository;
            }
        }

        public IRepository<FileModel> FileModels
        {
            get
            {
                if (fileRepository == null)
                    fileRepository = new FileRepository(db);
                return fileRepository;
            }
        }


        public void Save()
        {
            db.SaveChanges();
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public Task SaveAsync()
        {
            throw new NotImplementedException();
        }
    }
}
