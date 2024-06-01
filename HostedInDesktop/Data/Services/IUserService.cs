using HostedInDesktop.Data.Models;
using HostedInDesktop.Data.Services.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostedInDesktop.Data.Services
{
    public interface IUserService
    {
        Task<User> GetUserById(string userId);
        Task<User> EditAccount(string userId, User userToEdit);
        Task<string> DeleteAccount(string userId);

        Task<string> SendEmailCode(GenericStringClass email);

        Task<string> VerifyCode(GenericStringClass code);

        Task<string> updatePassword(RecoverPassswordRequest recoverPasssword, string token );
    }
}
