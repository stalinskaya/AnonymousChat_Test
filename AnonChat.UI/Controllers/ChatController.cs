using System;
using System.Collections.Generic;
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
        private class MessageEqualityComparer : IEqualityComparer<ChatMessage>
        {
            public bool Equals(ChatMessage m1, ChatMessage m2)
            {
                return (m1.Receiver.Id == m2.Receiver.Id && m1.Sender.Id == m2.Sender.Id) ||
                    (m1.Receiver.Id == m2.Sender.Id && m1.Sender.Id == m2.Receiver.Id);
            }

            public int GetHashCode(ChatMessage message)
            {
                if (message.Sender.Id.CompareTo(message.Receiver.Id) >= 0)
                    return message.Sender.Id.GetHashCode();
                else
                    return message.Receiver.Id.GetHashCode();
            }
        }

        //[Authorize]
        //public async Task<ActionResult> AddFriend(string id)
        //{
        //    ApplicationUser firstUser = await UserManager.FindByIdAsync(AuthenticationManager.User.Identity.GetUserId());
        //    ApplicationUser secondUser = await UserManager.FindByIdAsync(id);
        //    firstUser.SecondFriends.Add(secondUser);
        //    await UserManager.UpdateAsync(firstUser);
        //    return RedirectToAction("Index", "Home", new { id = id });
        //}

        [Authorize]
        [HttpGet]
        [Route("Dialogs")]
        public async Task<ActionResult> Dialogs()
        {
            ApplicationUser user = await accountService.FindUserById(User.Claims.First(c => c.Type == "UserID").Value);
            var messages = user.ReceivedMessages.Union(user.SendedMessages).ToList();
            messages.Sort((m1, m2) => m2.SendingTime.CompareTo(m1.SendingTime));
            IEnumerable<ChatMessage> dialogs = messages.Distinct(new MessageEqualityComparer());
            if (dialogs != null || dialogs.Count() != 0) return Ok(dialogs);
            else return BadRequest();
        }

        [Authorize]
        [HttpGet]
        [Route("Dialog/{id_1}/{id_2}")]
        public async Task<ActionResult> Dialog(string id_1, string id_2)
        {
            ApplicationUser firstUser = await accountService.FindUserById(User.Claims.First(c => c.Type == "UserID").Value);
            var secondUserId = firstUser.Id == id_1 ? id_2 : id_1;
            var dialog = firstUser.ReceivedMessages
                .Union(firstUser.SendedMessages)
                .Where(u => (u.SenderId == secondUserId) || (u.ReceiverId == secondUserId))
                .ToList();
            dialog.Sort((m1, m2) => m2.SendingTime.CompareTo(m1.SendingTime));
            //ViewBag.NewMessageSenderId = firstUser.Id;
            ////ViewBag.SenderPhoto = firstUser.Photo;
            ////ViewBag.SenderName = firstUser.UserName;
            //ViewBag.NewMessageReceiverId = secondUserId;
            return Ok(dialog);
        }

        [Authorize]
        [HttpPost("UserSearch")]
        public ActionResult UserSearch([FromBody]SearchViewModel searchViewModel)
        {
            timer1.Interval = (15000);
            timer1.Enabled = true;
            timer1.Start();
            var user = accountService.FindUserById(User.FindFirst(ClaimTypes.NameIdentifier).Value).Result;
            accountService.EditUserStatus(user, true);
            TimerCallback tm = new TimerCallback(Search);
            Timer timer2 = new Timer(tm, searchViewModel, 0, 2000);
        }

        public void Search(object obj)
        {
            SearchViewModel x = (SearchViewModel)obj;
            IEnumerable<ApplicationUser> users = accountService.GetUsers();
            if (x.AgeMin != null)
                users = users.Where(u => EF.Functions.DateDiffYear(u.BirthDay, DateTime.Today) >= x.AgeMin);
            if (x.AgeMax != null)
                users = users.Where(u => EF.Functions.DateDiffYear(u.BirthDay, DateTime.Today) <= x.AgeMax);
            if (!String.IsNullOrEmpty(x.Gender))
                users = users.Where(u => u.Gender == x.Gender);
            if (users.Count() > 1)
            {
                var user = users.First(c => c.StartSearch == users.Max(n => n.StartSearch));
            }
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