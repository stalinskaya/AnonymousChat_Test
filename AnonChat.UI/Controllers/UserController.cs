using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnonChat.BLL.Interfaces;
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

        public UserController(IAccountService serv)
        {
            this.accountService = serv;
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
        //GET : /api/UserProfile
        public async Task<Object> ChangeUserProfile()
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
    }
}