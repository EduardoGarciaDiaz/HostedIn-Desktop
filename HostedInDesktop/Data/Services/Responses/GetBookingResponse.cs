using HostedInDesktop.Data.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostedInDesktop.Data.Services.Responses
{
    public class GetBookingResponse
    {
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("bookings")]
        public List<Booking> Bookings { get; set; }
    }
}
