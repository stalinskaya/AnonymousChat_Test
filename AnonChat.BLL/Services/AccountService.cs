using AnonChat.BLL.Infrastructure;
using AnonChat.BLL.Interfaces;
using AnonChat.BLL.Settings;
using AnonChat.DAL.Interfaces;
using AnonChat.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace AnonChat.BLL.Services
{
    public class AccountService : IAccountService
    {
        IUnitOfWork Database { get; set; }
        private UserManager<ApplicationUser> UserManager;
        private SignInManager<ApplicationUser> SignInManager;
        private readonly ApplicationSettings applicationSettings;
        private readonly IEmailService emailService;

        public AccountService(IUnitOfWork uow, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IOptions<ApplicationSettings> options, IEmailService emailService)
        {
            Database = uow;
            UserManager = userManager;
            SignInManager = signInManager;
            this.applicationSettings = options.Value;
            this.emailService = emailService;
        }

        public async Task<object> Create(ApplicationUser newUser, string password, string url)
        {
            var user = await UserManager.FindByEmailAsync(newUser.Email);
            if (user == null)
            {
                user = new ApplicationUser
                {
                    Email = newUser.Email,
                    UserName = newUser.Email,
                    FirstName = newUser.FirstName,
                    LastName = newUser.LastName,
                    Sex = newUser.Sex,
                    Age = newUser.Age,
                    StatusSearch = false
                };
                try
                {
                    var result = await UserManager.CreateAsync(user, password);
                    if (!result.Succeeded)
                        return null;

                    await UserManager.AddToRoleAsync(user, "member");

                    var code = await UserManager.GenerateEmailConfirmationTokenAsync(user);
                    var encode = HttpUtility.UrlEncode(code);
                    var callbackUrl = new StringBuilder("http://")
                        .AppendFormat(url)
                        .AppendFormat("/api/account/ConfirmEmail")
                        .AppendFormat($"?userId={user.Id}&code={encode}");

                    await emailService.SendEmailAsync(user.Email, "Confirm your account",
                        $"Confirm the registration by clicking on the link: <a href='{callbackUrl}'>link</a>");
                    return result;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
                return null;
        }

        public async Task<ApplicationUser> FindUserById(string userId)
        {
            var user = await UserManager.FindByIdAsync(userId);
            if (user != null)
                return user;
            return null;
        }

        public async Task<ApplicationUser> FindUserByName(string userName)
        {
            var user = await UserManager.FindByNameAsync(userName);
            if (user != null)
                return user;
            return null;
        }

        public async Task<ApplicationUser> FindUserByEmail(string email)
        {
            var user = await UserManager.FindByEmailAsync(email);
            if (user != null)
                return user;
            return null;
        }

        public async Task<OperationDetails> ConfirmEmail(ApplicationUser user, string code)
        {
            var success = await UserManager.ConfirmEmailAsync(user, code);
            return success.Succeeded ? new OperationDetails(true, "Success", "") : new OperationDetails(false, "Error", "");
        }

        public async Task<object> Login(ApplicationUser new_user, string password)
        {
            var user = await UserManager.FindByNameAsync(new_user.UserName);
            if (user != null
                && await UserManager.CheckPasswordAsync(user, password)
                && await UserManager.IsEmailConfirmedAsync(user))
            {
                var role = await UserManager.GetRolesAsync(user);
                var options = new IdentityOptions();

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim("UserID",user.Id.ToString()),
                        new Claim(options.ClaimsIdentity.RoleClaimType,role.FirstOrDefault())
                    }),
                    Expires = DateTime.UtcNow.AddDays(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(applicationSettings.JWT_Secret)), SecurityAlgorithms.HmacSha256Signature)
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                var token = tokenHandler.WriteToken(securityToken);
                return token;
            }
            else
                return null;
        }

        public async Task<OperationDetails> EmailConfirmed(ApplicationUser user)
        {
            var confirm = await UserManager.IsEmailConfirmedAsync(user);
            if (!confirm)
                return new OperationDetails(false, "Error", "");
            return new OperationDetails(true, "Success", "");
        }


        public async Task<OperationDetails> CheckPassword(ApplicationUser user, string password)
        {
            var success = await UserManager.CheckPasswordAsync(user, password);
            if (!success)
                return new OperationDetails(false, "Error", "");
            return new OperationDetails(true, "Success", "");
        }

        public async Task<OperationDetails> Exit()
        {
            await SignInManager.SignOutAsync();
            return new OperationDetails(true, "Successful exit", "");
        }

        public IEnumerable<ApplicationUser> GetUsers()
        {
            return Database.Users.GetAll();
        }
    }
}
