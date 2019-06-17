using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnonChat.Models
{
    public class ChatMessage
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public bool IsReaded { get; set; }
        public DateTime SendingTime { get; set; }
        [ForeignKey("ApplicationUser")]
        public string SenderId { get; set; }
        public virtual ApplicationUser Sender { get; set; }
        [ForeignKey("ApplicationUser")]
        public string ReceiverId { get; set; }
        public virtual ApplicationUser Receiver { get; set; }
        [ForeignKey("Chat")]
        public string ChatId { get; set; }
        public virtual Chat Chat { get; set; }
    }
}
