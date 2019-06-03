using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnonChat.BLL.Interfaces;
using AnonChat.Models;
using AnonChat.UI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AnonChat.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        public readonly IAccountService accountService;
        public readonly IEmailService emailService;
        readonly IHostingEnvironment _appEnvironment;

        public AccountController(IAccountService serv, IEmailService _emailService, IHostingEnvironment hostingEnvironment)
        {
            accountService = serv;
            emailService = _emailService;
            _appEnvironment = hostingEnvironment;
        }

        [Authorize
        //(Roles = "Admin")
        ]
        public IEnumerable<ApplicationUser> Get()
        {
            return accountService.GetUsers();
        }

        [Authorize
        //(Roles = "Admin")
        ]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApplicationUser), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Get(string id)
        {
            var user = accountService.FindUserById(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost, Route("Register")]
        public async Task<object> Register([FromForm]RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var user = new ApplicationUser
            {
                Email = model.Email,
                UserName = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName
            };

            var url = HttpContext.Request.Host.ToString();
            var result = await accountService.Create(user, model.Password, url);
            if (result == null)
                return BadRequest(new { message = "Error" });
            return Ok(result);
        }

        //GET: /api/account/ConfirmEmail?userid=value&code=value
        [HttpGet]
        [Route("ConfirmEmail", Name = "ConfirmEmailRoute")]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            var user = await accountService.FindUserById(userId);
            if (user == null)
            {
                return BadRequest("Error");
            }
            var result = await accountService.ConfirmEmail(user, code);
            if (result.Succeeded)
                return Ok();
            return BadRequest(result.Message);
        }

        [HttpPost]
        [Route("Login")]
        //POST : /api/ApplicationUser/Login
        public async Task<IActionResult> Login([FromForm]LoginViewModel model)
        {
            var user = new ApplicationUser
            {
                Email = model.Email,
                UserName = model.Email
            };
            var token = await accountService.Login(user, model.Password);
            if (token != null)
                return Ok(new { token });
            return BadRequest(new { message = "Username or password is incorrect or not confirm email." });
        }
    }
}