using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostedInDesktop.Data.Services.Responses
{
    public class RecoverPasswordRepsonse
    {
        public string message { get; set; }
    }

    public class GenericStringClass
    {
        public string content { get; set; }

    }

    public class RecoverPassswordRequest 
    { 
        public string newPassword {  get; set; }
        public string email { get; set; }
    }
}
