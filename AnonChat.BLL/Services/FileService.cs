using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AnonChat.BLL.Interfaces;
using AnonChat.DAL.Interfaces;
using AnonChat.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace AnonChat.BLL.Services
{
    public class FileService : IFileService
    {
        private IUnitOfWork Database { get; set; }

        private readonly IHostingEnvironment _hostingEnvironment;

        public FileService(IUnitOfWork uow,
            IHostingEnvironment hostingEnvironment)
        {
            Database = uow;
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task EditPhoto(ApplicationUser user, IFormFile photo)
        {
            var createPath = _hostingEnvironment.WebRootPath + "/File/" + user.Id;
            if (!Directory.Exists(createPath))
                Directory.CreateDirectory(createPath);

            using (var fileStream = new FileStream(createPath, FileMode.Create))
            {
                await photo.CopyToAsync(fileStream);
            }

            var new_photo = new FileModel
            {
                ApplicationUserID = user.Id,
                Name = photo.FileName,
                Path = createPath
            };
            Database.FileModels.Create(new_photo);
            Database.Save();

            user.Photo = new_photo;
            Database.Users.Update(user);
            Database.Save();
        }
    }
}