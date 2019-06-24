
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AnonChat.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDay { get; set; }
        public string Gender { get; set; }
        public virtual ICollection<Chat> SentChats { get; set; }
        public virtual ICollection<Chat> ReceivedChats { get; set; }
        public virtual ICollection<ChatMessage> SendedMessages { get; set; }
        public bool StatusSearch { get; set; }
        public DateTime? StartSearch { get; set; }

        [ForeignKey("FileModel")]
        public int? PhotoID { get; set; }
        public virtual FileModel Photo { get; set; }
        public ApplicationUser()
        {
            SentChats = new List<Chat>();
            ReceivedChats = new List<Chat>();
            SendedMessages = new List<ChatMessage>();
        }
    }
}
