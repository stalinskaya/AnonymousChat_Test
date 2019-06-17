using AnonChat.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnonChat.UI.ViewModels
{
    public class EditPhotoViewModel
    {
        public ApplicationUser ApplicationUser { get; set; }
        public IFormFile Photo { get; set; }
        public EditPhotoViewModel(ApplicationUser user, IFormFile photo)
        {
            ApplicationUser = user;
            Photo = photo;
        }
    }
}
