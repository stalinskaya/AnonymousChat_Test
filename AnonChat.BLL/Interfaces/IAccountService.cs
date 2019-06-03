using AnonChat.BLL.Infrastructure;
using AnonChat.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AnonChat.BLL.Interfaces
{
    public interface IAccountService
    {
        Task<object> Create(ApplicationUser newUser, string password, string url);
        Task<object> Login(ApplicationUser new_user, string password);
        Task<ApplicationUser> FindUserById(string userId);
        Task<ApplicationUser> FindUserByName(string userName);
        Task<ApplicationUser> FindUserByEmail(string email);
        Task<OperationDetails> ConfirmEmail(ApplicationUser user, string code);
        Task<OperationDetails> EmailConfirmed(ApplicationUser user);
        Task<OperationDetails> CheckPassword(ApplicationUser user, string password);
        IEnumerable<ApplicationUser> GetUsers();
        Task<OperationDetails> Exit();
    }
}
