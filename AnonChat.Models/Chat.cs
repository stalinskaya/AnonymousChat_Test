using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AnonChat.Models
{
    public class Chat
    {
        public string ChatID { get; set; }
        public virtual ICollection<ChatMessage> Messages { get; set; }
        public virtual ICollection<UserChat> UserChats { get; set; }
        public bool StatusBlock { get; set; }
        public bool StatusAnonymity { get; set; }

    }
}
