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
    public class ApplicationUserRepository : IRepository<ApplicationUser>
    {
        private ChatContext db;

        public ApplicationUserRepository(ChatContext context)
        {
            this.db = context;
        }

        public IEnumerable<ApplicationUser> GetAll()
        {
            return db.Users;
        }

        public ApplicationUser Get(int id)
        {
            return db.Users.Find(id);
        }

        public void Create(ApplicationUser user)
        {
            db.Users.Add(user);
        }

        public void Update(ApplicationUser user)
        {
            db.Entry(user).State = EntityState.Modified;
        }
        public IEnumerable<ApplicationUser> Find(Func<ApplicationUser, Boolean> predicate)
        {
            return db.Users.Where(predicate).ToList();
        }
        public void Delete(int id)
        {
            ApplicationUser user = db.Users.Find(id);
            if (user != null)
                db.Users.Remove(user);
        }
    }
}
