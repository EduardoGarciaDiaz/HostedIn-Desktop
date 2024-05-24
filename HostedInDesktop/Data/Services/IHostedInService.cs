using HostedInDesktop.Data.Models;
using HostedInDesktop.Data.Services.Responses;

namespace HostedInDesktop.Data.Services
{
    interface IHostedInService
    {
        public Task LoginAsync (User user);

    }
}
