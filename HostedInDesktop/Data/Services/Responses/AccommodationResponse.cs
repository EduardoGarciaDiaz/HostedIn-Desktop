using HostedInDesktop.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostedInDesktop.Data.Services.Responses
{
    public class AccommodationResponse
    {
        public string Message { get; set; }
        public Accommodation Accommodation { get; set; }
    }
}
