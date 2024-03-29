﻿using AnonChat.DAL.Converters;
using AnonChat.Models;
using Microsoft.AspNetCore.Identity;
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
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<FileModel> FileModels { get; set; }

        public ChatContext(DbContextOptions<ChatContext> options)
            : base(options)
        {
            //Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>(entity => entity.Property(m => m.Id).HasMaxLength(127));
            builder.Entity<IdentityRole>(entity => entity.Property(m => m.Id).HasMaxLength(127));
            builder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.Property(m => m.LoginProvider).HasMaxLength(127);
                entity.Property(m => m.ProviderKey).HasMaxLength(127);
            });
            builder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.Property(m => m.UserId).HasMaxLength(127);
                entity.Property(m => m.RoleId).HasMaxLength(127);
            });
            builder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.Property(m => m.UserId).HasMaxLength(127);
                entity.Property(m => m.LoginProvider).HasMaxLength(127);
                entity.Property(m => m.Name).HasMaxLength(127);
            });

            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(bool))
                    {
                        property.SetValueConverter(new BoolToIntConverter());
                    }
                }
            }

            builder.Entity<ChatMessage>()
                .HasOne<ApplicationUser>(m => m.Reciever)
                .WithMany(u => u.SendedMessages)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<ChatMessage>()
                .HasOne<Chat>(m => m.Chat)
                .WithMany(u => u.Messages)
                .OnDelete(DeleteBehavior.SetNull);
            builder.Entity<Chat>()
                .HasOne<ApplicationUser>(m => m.Sender)
                .WithMany(u => u.SentChats)
                .OnDelete(DeleteBehavior.SetNull);
            builder.Entity<Chat>()
                .HasOne<ApplicationUser>(m => m.Receiver)
                .WithMany(u => u.ReceivedChats)
                .OnDelete(DeleteBehavior.SetNull);


        }
    }
}
