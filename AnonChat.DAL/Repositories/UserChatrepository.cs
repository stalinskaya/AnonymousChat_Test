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
    public class UserChatRepository : IRepository<UserChat>
    {
        private ChatContext db;

        public UserChatRepository(ChatContext context)
        {
            this.db = context;
        }

        public IEnumerable<UserChat> GetAll()
        {
            return db.UserChats;
        }

        public UserChat Get(int id)
        {
            return db.UserChats.Find(id);
        }

        public void Create(UserChat userchat)
        {
            db.UserChats.Add(userchat);
        }

        public void Update(UserChat userchat)
        {
            db.Entry(userchat).State = EntityState.Modified;
        }
        public IEnumerable<UserChat> Find(Func<UserChat, Boolean> predicate)
        {
            return db.UserChats.Where(predicate).ToList();
        }
        public void Delete(int id)
        {
            UserChat userChat = db.UserChats.Find(id);
            if (userChat != null)
                db.UserChats.Remove(userChat);
        }
    }
}
