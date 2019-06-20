using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.WebSockets;
using System.Security.Claims;
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
        public async Task<Object> UserSearch([FromBody] SearchViewModel searchViewModel)
        {
            string text = await Task.FromResult<string>(GetSearchUserId(searchViewModel).Result);

        }
        private async Task<string> GetSearchUserId(SearchViewModel searchViewModel)
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
                using (WebSocket socket = new WebSocket())
                {

                    if (full_match.Count >= 1)
                    {
                        return full_match.Find(u => u.searchStart == (full_match.Min(i => i.searchStart))).userId;
                    }
                    else return full_match.First().userId;
                }
                
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
        public static void CallToChildThread()
        {
            Console.WriteLine("Child thread starts");
        }
            
        }

    }
}