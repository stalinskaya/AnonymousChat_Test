using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AnonChat.UI.ViewModels
{
    public class EditUserViewModel
    {
        [Required]
        [Display(Name = "FirstName")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "LastName")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Дата рождения")]
        public DateTime Birthday { get; set; }

        [Required]
        [Display(Name = "Пол")]
        public string Gender { get; set; }
    }
}
