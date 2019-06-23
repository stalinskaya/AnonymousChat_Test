﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AnonChat.UI.ViewModels
{
    public class SendMessageViewModel
    {
        [Required]
        [Display(Name = "Message")]
        public string Message { get; set; }

        [Required]
        [Display(Name = "ReceiverId")]
        public string ReceiverId { get; set; }

        [Required]
        [Display(Name = "ChatID")]
        public string ChatId { get; set; }
    }
}
