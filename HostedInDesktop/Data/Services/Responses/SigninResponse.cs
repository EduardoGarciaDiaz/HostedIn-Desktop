
using HostedInDesktop.Data.Models;

namespace HostedInDesktop.Data.Services.Responses
{
    public class SigninResponse
    {
        public string message { get; set; }
        public User user { get; set; }
    }
}
