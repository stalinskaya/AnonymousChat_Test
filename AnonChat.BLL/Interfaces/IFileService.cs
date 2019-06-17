using AnonChat.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AnonChat.BLL.Interfaces
{
    public interface IFileService
    {
        Task EditPhoto(ApplicationUser user, IFormFile photo);
    }
}
