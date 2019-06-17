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
    public class FileRepository : IRepository<FileModel>
    {
        private ChatContext db;

        public FileRepository(ChatContext context)
        {
            this.db = context;
        }

        public IEnumerable<FileModel> GetAll()
        {
            return db.FileModels;
        }

        public FileModel Get(int id)
        {
            return db.FileModels.Find(id);
        }

        public void Create(FileModel file)
        {
            db.FileModels.Add(file);
        }

        public void Update(FileModel file)
        {
            db.Entry(file).State = EntityState.Modified;
        }
        public IEnumerable<FileModel> Find(Func<FileModel, Boolean> predicate)
        {
            return db.FileModels.Where(predicate).ToList();
        }
        public void Delete(int id)
        {
            FileModel file = db.FileModels.Find(id);
            if (file != null)
                db.FileModels.Remove(file);
        }
    }
}
