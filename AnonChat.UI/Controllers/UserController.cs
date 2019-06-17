using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnonChat.BLL.Interfaces;
using AnonChat.Models;
using AnonChat.UI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AnonChat.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public readonly IAccountService accountService;
        public readonly IFileService fileService;

        public UserController(IAccountService serv, IFileService fileService)
        {
            this.accountService = serv;
            this.fileService = fileService;
        }

        [HttpGet]
        [Authorize]
        //GET : /api/UserProfile
        public async Task<Object> GetUserProfile()
        {
            string userId = User.Claims.First(c => c.Type == "UserID").Value;
            var user = await accountService.FindUserById(userId);
            return new
            {
                user.FirstName,
                user.LastName,
                user.Email,
                user.UserName,
                user.BirthDay,
                user.Gender
            };
        }

        [HttpPut]
        [Authorize]
        //GET : /api/User
        public async Task<Object> ChangeUserProfile(EditUserViewModel editUser)
        {
            string userId = User.Claims.First(c => c.Type == "UserID").Value;
            var user = await accountService.FindUserById(userId);
            var new_user = new ApplicationUser
            {
                FirstName = editUser.FirstName,
                LastName = editUser.LastName,
                Gender = editUser.Gender,
                BirthDay = editUser.Birthday,
            };
            accountService.EditUser(user, new_user);
            return Ok(user);
        }

        [Authorize]
        [HttpPost("EditPhoto")]
        public async Task<object> EditPhoto (EditPhotoViewModel editPhoto)
        {
            await fileService.EditPhoto(editPhoto.ApplicationUser, editPhoto.Photo);
            return Ok("Changed");
        }
    }
}