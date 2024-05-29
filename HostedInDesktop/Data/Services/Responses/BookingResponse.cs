using HostedInDesktop.Data.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostedInDesktop.Data.Services.Responses
{
    public class BookingResponse
    {
        public string message { get; set; }

        public Booking booking { get; set; }
    }
}
