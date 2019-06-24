using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.WebSockets;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AnonChat.BLL.Hubs;
using AnonChat.BLL.Interfaces;
using AnonChat.Models;
using AnonChat.UI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace AnonChat.UI.Controllers
{
    public class SearchLine
    {
        public ApplicationUser user { get; set; }
        public string gender { get; set; }
        public int age_max { get; set; }
        public int age_min { get; set; }
        public DateTime searchStart { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private static Dictionary<string, SearchLine> _searchList = new Dictionary<string, SearchLine>();


        public readonly IAccountService accountService;
        public readonly IChatService chatService;
        private readonly IHubContext<ChatHub> chatHub;

        public ChatController(IAccountService accountService, IChatService chatService, IHubContext<ChatHub> chatHub)
        {
            this.accountService = accountService;
            this.chatService = chatService;
            this.chatHub = chatHub;
        }

        [HttpPost("UserSearch")]
        public async Task<string> UserSearch([FromBody] SearchViewModel searchViewModel)
        {
            var resultId = "";
            ApplicationUser user = await accountService.FindUserById(User.Claims.First(c => c.Type == "UserID").Value);
            string userId = user.Id;
            SearchLine searchline = new SearchLine()
            {
                user = user,
                gender = searchViewModel.Gender,
                age_max = searchViewModel.AgeMax,
                age_min = searchViewModel.AgeMin,
                searchStart = DateTime.Now
            };
            _searchList[userId] = searchline;
            await Task.Run(() =>
            {
                var isEnd = false;
                var child = Task.Run(() =>
                {
                    while (!isEnd)
                    {
                        List<SearchLine> list = new List<SearchLine>();
                        var searchingRightNow = _searchList.Select(d => d.Value).ToList().FindAll(u => EF.Functions.DateDiffYear(searchline.user.BirthDay, DateTime.Today) >= u.age_min &&
                                                                    EF.Functions.DateDiffYear(searchline.user.BirthDay, DateTime.Today) <= u.age_max &&
                                                                    u.gender == searchline.user.Gender &&
                                                                    EF.Functions.DateDiffSecond(u.searchStart, DateTime.Today) <= 30
                                                                    && u.user.Id != searchline.user.Id);
                        var full_match = new List<SearchLine>();
                        if (searchingRightNow.Any())
                        {
                            full_match = searchingRightNow.FindAll(u => EF.Functions.DateDiffYear(u.user.BirthDay, DateTime.Today) >= searchline.age_min &&
                                                                      EF.Functions.DateDiffYear(u.user.BirthDay, DateTime.Today) <= searchline.age_max &&
                                                                      searchline.gender == (u.user.Gender));
                        }


                        if (full_match.Count > 1)
                        {
                            resultId = full_match.OrderBy(sl => sl.searchStart).FirstOrDefault().user.Id;
                            break;
                        }
                        else if (full_match.Count == 1)
                        {
                            resultId = full_match.First().user.Id;
                            break;
                        }
                    }
                });
                for (int i = 0; i < 6; i++)
                {
                    Thread.Sleep(5000);

                    if (child.IsCompleted)
                    {
                        break;
                    }
                }
                isEnd = true;
            });
            return JsonConvert.SerializeObject(resultId);
        }
               

        //GET: api/dialog/GetAllDialogs
        [HttpGet, Route("GetAllDialogs")]
        public async Task<IActionResult> GetAll()
        {
            var userId = User.Claims.First(c => c.Type == "UserID").Value;
            var dialogs = await chatService.FindAllDialogs(userId);
            if (dialogs == null)
            {
                return BadRequest(new { message = "Error, you have no dialogs yet" });
            }
            return Ok(dialogs);
        }

        //GET: api/dialog/DialogDetails/id
        [HttpGet, Route("DialogDetails/{userid}")]
        public async Task<IActionResult> Get(string userId)
        {
            var myId = User.Claims.First(c => c.Type == "UserID").Value;
            if (await chatService.ExistChat(myId, userId) == false) chatService.AddChat(userId, myId);
            var dialog = await chatService.GetDialog(myId, userId);
            var result = dialog.Messages;
            if (result == null)
            {
                return Ok("No messages yet");
            }
            return Ok(result);
        }

        //POST: api/dialog/SendMessage
        [HttpPost, Route("SendMessage")]
        public async Task<IActionResult> SendMessage([FromBody] SendMessageViewModel model)
        {
            try
            {
                UserIds receiver, caller;
                FindCallerReceiverByIds(model.ReceiverId, out caller, out receiver);

                var newMessage = chatService.AddChatMessage(caller.userId, model.Message, model.ChatId);
                //var dialog = chatService.GetDialog(model.ChatId);

                await chatHub.Clients.Client(caller.connId).SendAsync("SendMyself", newMessage);

                if (receiver != null)
                {
                    await chatHub.Clients.Client(receiver.connId)
                        .SendAsync("Send", newMessage, caller.userId);

                    //await Clients.Client(receiver.ConnId).SendAsync("SoundNotify", "");
                }

                return Ok();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private void FindCallerReceiverByIds(string receiverId, out UserIds caller, out UserIds receiver)
        {
            var connId = ChatHub.usersList
                .Where(x => x.userId == User.Claims.First(c => c.Type == "UserID").Value)
                .Select(x => x.connId)
                .First();

            receiver = ChatHub.usersList.Find(r => r.userId == receiverId);
            caller = ChatHub.usersList.Find(c => c.connId == connId);
        }
    }
}