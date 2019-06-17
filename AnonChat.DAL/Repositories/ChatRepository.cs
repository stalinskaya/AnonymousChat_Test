using AnonChat.DAL.EF;
using AnonChat.DAL.Interfaces;
using AnonChat.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnonChat.DAL.Repositories
{
    public class ChatRepository : IRepository<Chat>
    {
        private ChatContext db;

        public ChatRepository(ChatContext context)
        {
            this.db = context;
        }

        public IEnumerable<Chat> GetAll()
        {
            return db.Chats;
        }

        public Chat Get(int id)
        {
            return db.Chats.Find(id);
        }

        public void Create(Chat chat)
        {
            db.Chats.Add(chat);
        }

        public void Update(Chat chat)
        {
            db.Entry(chat).State = EntityState.Modified;
        }
        public IEnumerable<Chat> Find(Func<Chat, Boolean> predicate)
        {
            return db.Chats.Where(predicate).ToList();
        }
        public void Delete(int id)
        {
            Chat chat = db.Chats.Find(id);
            if (chat != null)
                db.Chats.Remove(chat);
        }
    }
}
