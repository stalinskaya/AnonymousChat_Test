using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AnonChat.Models
{
    public class Chat
    {
        public string ChatID { get; set; }
        [ForeignKey("ApplicationUser")]
        public string FirstId { get; set; }
        [ForeignKey("ApplicationUser")]
        public string SecondId { get; set; }
        public virtual ApplicationUser First { get; set; }
        public virtual ApplicationUser Second { get; set; }
        public virtual ICollection<ChatMessage> Messages { get; set; }
        public bool StatusBlock { get; set; }
        public bool StatusAnonymity { get; set; }

    }
}
