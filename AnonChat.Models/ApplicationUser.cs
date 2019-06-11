using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnonChat.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDay { get; set; }
        public string Gender { get; set; }

        public virtual ICollection<ChatMessage> SendedMessages { get; set; }
        public virtual ICollection<ChatMessage> ReceivedMessages { get; set; }
        public bool StatusSearch { get; set; }
        public DateTime StartSearch { get; set; }
        public ApplicationUser()
        {
            SendedMessages = new List<ChatMessage>();
            ReceivedMessages = new List<ChatMessage>();
        }
    }
}
