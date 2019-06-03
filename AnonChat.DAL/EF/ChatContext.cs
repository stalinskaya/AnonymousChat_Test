using AnonChat.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnonChat.DAL.EF
{
    public class ChatContext : IdentityDbContext<ApplicationUser>
    {

        public DbSet<LogDetail> LogDetails { get; set; }

        public ChatContext(DbContextOptions<ChatContext> options)
            : base(options)
        {
            //Database.EnsureCreated();
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

    }
}
