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
using AnonChat.BLL.Interfaces;
using AnonChat.Models;
using AnonChat.UI.Hubs;
using AnonChat.UI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AnonChat.UI.Controllers
{
    public class SearchIds
    {
        public string userId { get; set; }
        public string connId { get; set; }
        public string gender { get; set; }
        public int age_max { get; set; }
        public int age_min { get; set; }
        public DateTime searchStart { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        public readonly IChatHub chatHub;
        public readonly IAccountService accountService;
        public readonly IChatService chatService;
        public ChatController (IAccountService accountService, IChatHub chatHub)
        {
            this.accountService = accountService;
            this.chatHub = chatHub;
        }

        static List<SearchIds> searchList = new List<SearchIds>();

        [HttpPost("UserSearch")]
        public IActionResult UserSearch([FromBody] SearchViewModel searchViewModel)
        {
            var context = ControllerContext.HttpContext;
            var isSocketRequest = context.WebSockets.IsWebSocketRequest;

            //WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
            Task.Run(() =>
            {
                var child = Task.Run(() =>
                {
                    while (true)
                    {
                        Task<ApplicationUser> userTask
                        = accountService.FindUserById(User.Claims.First(c => c.Type == "UserID").Value);

                        userTask.Wait();

                        ApplicationUser user = userTask.Result;

                        UpdateSearchList(user.Id, searchViewModel.Gender, searchViewModel.AgeMax, searchViewModel.AgeMin, DateTime.Now);
                        var searchingRN = searchList.FindAll(u => EF.Functions.DateDiffYear(user.BirthDay, DateTime.Today) >= u.age_min &&
                                                                  EF.Functions.DateDiffYear(user.BirthDay, DateTime.Today) <= u.age_max &&
                                                                  u.gender == user.Gender);
                        var full_match = new List<SearchIds>();
                        if (searchingRN.Any())
                        {
                            full_match = searchingRN.FindAll(u => EF.Functions.DateDiffYear((accountService.FindUserById(u.userId).Result.BirthDay), DateTime.Today) >= u.age_min &&
                                                                      EF.Functions.DateDiffYear((accountService.FindUserById(u.userId).Result.BirthDay), DateTime.Today) <= u.age_max &&
                                                                      u.gender == (accountService.FindUserById(u.userId)).Result.Gender);
                        }
                        var resultId = "";
                        if (full_match.Count >= 1)
                        {
                            resultId = full_match.Find(u => u.searchStart == (full_match.Min(i => i.searchStart))).userId;
                        }
                        else resultId = full_match.First().userId;
                        var bytes = Encoding.ASCII.GetBytes(resultId);
                        //var arraySegment = new ArraySegment<byte>(bytes);
                        //await webSocket.SendAsync(arraySegment, WebSocketMessageType.Text, true, CancellationToken.None);

                        Thread.Sleep(2000); //sleeping so that we can see several messages are sent
                    }
                });

                for (int i = 0; i < 30; i++)
                {
                    Thread.Sleep(1000);

                    if(child.IsCompleted)
                    {
                        break;
                    }
                }


            });
            //await GetSearchUserId(context, webSocket, searchViewModel);
            
            return Ok();
            
        }
        private async Task GetSearchUserId(HttpContext context, WebSocket webSocket, SearchViewModel searchViewModel)
        {
            while (true)
            {
                var user = await accountService.FindUserById(User.Claims.First(c => c.Type == "UserID").Value);
                UpdateSearchList(user.Id, searchViewModel.Gender, searchViewModel.AgeMax, searchViewModel.AgeMin, DateTime.Now);
                var searchingRN = searchList.FindAll(u => EF.Functions.DateDiffYear(user.BirthDay, DateTime.Today) >= u.age_min &&
                                                          EF.Functions.DateDiffYear(user.BirthDay, DateTime.Today) <= u.age_max &&
                                                          u.gender == user.Gender);
                var full_match = new List<SearchIds>();
                if (searchingRN.Any())
                {
                    full_match = searchingRN.FindAll(u => EF.Functions.DateDiffYear((accountService.FindUserById(u.userId).Result.BirthDay), DateTime.Today) >= u.age_min &&
                                                              EF.Functions.DateDiffYear((accountService.FindUserById(u.userId).Result.BirthDay), DateTime.Today) <= u.age_max &&
                                                              u.gender == (accountService.FindUserById(u.userId)).Result.Gender);
                }
                var resultId = "";
                if (full_match.Count >= 1)
                {
                    resultId = full_match.Find(u => u.searchStart == (full_match.Min(i => i.searchStart))).userId;
                }
                else resultId = full_match.First().userId;
                var bytes = Encoding.ASCII.GetBytes(resultId);
                var arraySegment = new ArraySegment<byte>(bytes);
                await webSocket.SendAsync(arraySegment, WebSocketMessageType.Text, true, CancellationToken.None);
                Thread.Sleep(2000); //sleeping so that we can see several messages are sent
            }
        }

        void UpdateSearchList(string searchId, string gender, int age_max, int age_min, DateTime searchStart)
        {
            searchList.RemoveAll(i => i.searchStart.AddSeconds(30) < searchStart);
            var index = searchList.FindIndex(i => i.userId == searchId);
            if (index != -1 && searchList[index].connId != chatHub.GetConnectionId())
            {
                searchList[index].connId = chatHub.GetConnectionId();
                searchList[index].gender = gender;
                searchList[index].age_max = age_max;
                searchList[index].age_min = age_min;
                searchList[index].searchStart = searchStart;
            }
            else
            {
                searchList.Add(new SearchIds { connId = chatHub.GetConnectionId(), userId = searchId, gender = gender, age_max = age_max, age_min = age_min });
            }
        }
        [HttpGet("{userId}")]
        public async Task<Object> GetUserProfile(string userId)
        {
            var user = await accountService.FindUserById(userId);
            return new
            {
                user.FirstName,
                user.LastName,
                user.Email,
                user.BirthDay,
                user.Gender
            };
        }
    }
}