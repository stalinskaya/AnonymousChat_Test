using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AnonChat.Models
{
    public class FileModel
    {
        [Key]
        public int FileID { get; set; }

        [ForeignKey("ApplicationUser")]
        public string ApplicationUserID { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        //[ForeignKey("ChatMessage")]
        //public int? ChatMessageId { get; set; }
        //public virtual ChatMessage ChatMessage { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
    }
}
