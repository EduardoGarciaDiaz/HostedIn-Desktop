using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostedInDesktop.Data.Services.Responses
{
    public class RecoverPasswordRepsonse
    {
        public string Message { get; set; }
    }

    public class GenericStringClass
    {
        public string Content { get; set; }

    }

    public class RecoverPassswordRequest 
    { 
        public string NewPassword {  get; set; }
        public string Email { get; set; }
    }
}
