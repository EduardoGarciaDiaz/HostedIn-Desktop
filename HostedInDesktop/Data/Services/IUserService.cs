using HostedInDesktop.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostedInDesktop.Data.Services
{
    public interface IUserService
    {

        Task<User> EditAccount(string userId, User userToEdit);
        Task<string> DeleteAccount(string userId);
    }
}
