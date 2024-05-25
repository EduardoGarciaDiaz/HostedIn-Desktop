using HostedInDesktop.Data.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostedInDesktop.Data.Services.Responses
{
    public class GetAccommodationsResponse
    {
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("accommodations")]
        public List<Accommodation> Accommodations { get; set;}
    }
}
