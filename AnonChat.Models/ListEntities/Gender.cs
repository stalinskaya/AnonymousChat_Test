using System;
using System.Collections.Generic;
using System.Text;

namespace AnonChat.Models.ListEntities
{
    public class Gender
    {
        public int GenderID { get; set; }
        public string GenderName { get; set; }
        public ICollection<ApplicationUser> ApplicationUsers { get; set; }
    }
}
