using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnonChat.UI.ViewModels
{
    public class SearchViewModel
    {
        public int? AgeMin { get; set; }
        public int? AgeMax { get; set; }
        public string Gender { get; set; }
    }
}
