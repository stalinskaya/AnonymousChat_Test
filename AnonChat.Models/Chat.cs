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

        [ForeignKey("ApplicationUser")]
        public string SenderId { get; set; }
        public virtual ApplicationUser Sender { get; set; }

        [ForeignKey("ApplicationUser")]
        public string ReceiverId { get; set; }
        public virtual ApplicationUser Receiver { get; set; }
        public bool StatusBlock { get; set; }
        public bool StatusAnonymity { get; set; }

    }
}
