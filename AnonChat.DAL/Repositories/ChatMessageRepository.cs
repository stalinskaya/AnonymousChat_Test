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
    public class ChatMessageRepository : IRepository<ChatMessage>
    {
        private ChatContext db;

        public ChatMessageRepository(ChatContext context)
        {
            this.db = context;
        }

        public IEnumerable<ChatMessage> GetAll()
        {
            return db.ChatMessages;
        }

        public ChatMessage Get(int id)
        {
            return db.ChatMessages.Find(id);
        }

        public void Create(ChatMessage chatMessage)
        {
            db.ChatMessages.Add(chatMessage);
        }

        public void Update(ChatMessage chatMessage)
        {
            db.Entry(chatMessage).State = EntityState.Modified;
        }
        public IEnumerable<ChatMessage> Find(Func<ChatMessage, Boolean> predicate)
        {
            return db.ChatMessages.Where(predicate).ToList();
        }
        public void Delete(int id)
        {
            ChatMessage chatMessage = db.ChatMessages.Find(id);
            if (chatMessage != null)
                db.ChatMessages.Remove(chatMessage);
        }
    }
}
