using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using AnonChat.BLL.Interfaces;
using AnonChat.Models;
using AnonChat.UI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AnonChat.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        
        public readonly IAccountService accountService;
        public readonly IChatService chatService;
        public ChatController (IAccountService accountService)
        {
            this.accountService = accountService;
        }

        [Authorize]
        [HttpPost("UserSearch")]
        public async Task<ActionResult> UserSearch([FromBody]SearchViewModel searchViewModel)
        {
            var user = await accountService.FindUserById(User.Claims.First(c => c.Type == "UserID").Value);
            accountService.EditUserStatus(user, true);
            IEnumerable<ApplicationUser> users = accountService.GetUsers();
            if (searchViewModel.AgeMin != null)
                users = users.Where(u => EF.Functions.DateDiffYear(u.BirthDay, DateTime.Today) >= searchViewModel.AgeMin);
            if (searchViewModel.AgeMax != null)
                users = users.Where(u => EF.Functions.DateDiffYear(u.BirthDay, DateTime.Today) <= searchViewModel.AgeMax);
            if (!String.IsNullOrEmpty(searchViewModel.Gender))
                users = users.Where(u => u.Gender == searchViewModel.Gender);
            accountService.EditUserStatus(user, false);
            if (users.Any() == false) return BadRequest("User не был найден");
            else return Ok(users.First().Id);
        }
    }
}