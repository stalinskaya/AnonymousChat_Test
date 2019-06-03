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
        public string Sex { get; set; }
        public int Age { get; set; }
        public bool StatusSearch { get; set; }
    }
}
