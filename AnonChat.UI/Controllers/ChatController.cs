using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Timers;
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
        //private class MessageEqualityComparer : IEqualityComparer<ChatMessage>
        //{
        //    public bool Equals(ChatMessage m1, ChatMessage m2)
        //    {
        //        return (m1.Receiver.Id == m2.Receiver.Id && m1.Sender.Id == m2.Sender.Id) ||
        //            (m1.Receiver.Id == m2.Sender.Id && m1.Sender.Id == m2.Receiver.Id);
        //    }

        //    public int GetHashCode(ChatMessage message)
        //    {
        //        if (message.Sender.Id.CompareTo(message.Receiver.Id) >= 0)
        //            return message.Sender.Id.GetHashCode();
        //        else
        //            return message.Receiver.Id.GetHashCode();
        //    }
        //}

        ////[Authorize]
        ////public async Task<ActionResult> AddFriend(string id)
        ////{
        ////    ApplicationUser firstUser = await UserManager.FindByIdAsync(AuthenticationManager.User.Identity.GetUserId());
        ////    ApplicationUser secondUser = await UserManager.FindByIdAsync(id);
        ////    firstUser.SecondFriends.Add(secondUser);
        ////    await UserManager.UpdateAsync(firstUser);
        ////    return RedirectToAction("Index", "Home", new { id = id });
        ////}

        //[Authorize]
        //[HttpGet]
        //[Route("Dialogs")]
        //public async Task<ActionResult> Dialogs()
        //{
        //    ApplicationUser user = await accountService.FindUserById(User.Claims.First(c => c.Type == "UserID").Value);
        //    var messages = user.ReceivedMessages.Union(user.SendedMessages).ToList();
        //    messages.Sort((m1, m2) => m2.SendingTime.CompareTo(m1.SendingTime));
        //    IEnumerable<ChatMessage> dialogs = messages.Distinct(new MessageEqualityComparer());
        //    if (dialogs != null || dialogs.Count() != 0) return Ok(dialogs);
        //    else return BadRequest();
        //}

        //[Authorize]
        //[HttpGet]
        //[Route("Dialog/{id_1}/{id_2}")]
        //public async Task<ActionResult> Dialog(string id_1, string id_2)
        //{
        //    ApplicationUser firstUser = await accountService.FindUserById(User.Claims.First(c => c.Type == "UserID").Value);
        //    var secondUserId = firstUser.Id == id_1 ? id_2 : id_1;
        //    var dialog = firstUser.ReceivedMessages
        //        .Union(firstUser.SendedMessages)
        //        .Where(u => (u.SenderId == secondUserId) || (u.ReceiverId == secondUserId))
        //        .ToList();
        //    dialog.Sort((m1, m2) => m2.SendingTime.CompareTo(m1.SendingTime));
        //    //ViewBag.NewMessageSenderId = firstUser.Id;
        //    ////ViewBag.SenderPhoto = firstUser.Photo;
        //    ////ViewBag.SenderName = firstUser.UserName;
        //    //ViewBag.NewMessageReceiverId = secondUserId;
        //    return Ok(dialog);
        //}

        [Authorize]
        [HttpPost("UserSearch")]
        public async Task<ActionResult> UserSearch([FromBody]SearchViewModel searchViewModel)
        {
            var user = await accountService.FindUserById(User.Claims.First(c => c.Type == "UserID").Value);
            accountService.EditUserStatus(user, true);
            IEnumerable<ApplicationUser> users = accountService.GetUsers();
            Stopwatch stopWatch = new Stopwatch();
            var counter = 0;
            do
            {
                stopWatch.Start();
                if (searchViewModel.AgeMin != null)
                    users = users.Where(u => EF.Functions.DateDiffYear(u.BirthDay, DateTime.Today) >= searchViewModel.AgeMin);
                if (searchViewModel.AgeMax != null)
                    users = users.Where(u => EF.Functions.DateDiffYear(u.BirthDay, DateTime.Today) <= searchViewModel.AgeMax);
                if (!String.IsNullOrEmpty(searchViewModel.Gender))
                    users = users.Where(u => u.Gender == searchViewModel.Gender);
                if (users.Count() > 1)
                {
                    users = users.Where(c => c.StartSearch == users.Max(n => n.StartSearch));
                }
                stopWatch.Stop();
                counter += stopWatch.Elapsed.Milliseconds;
            }
            while (counter <= 30000 && users.Any() == false);
            accountService.EditUserStatus(user, false);
            if (users.Any() == false) return BadRequest("User не был найден");
            else return Ok(users);
        }


        [Authorize]
        [HttpPost("SendMessage")]
        public async Task<ActionResult> SendMessage(ChatMessage message)
        {
            message.SendingTime = DateTime.Now;
            message.IsReaded = false;
            chatService.AddMessage(message);
            var sender = await accountService.FindUserById(message.SenderId);
            var msg = sender.SendedMessages.First(m => m.Id == message.Id);
            return Ok(msg);
        }
    }
}