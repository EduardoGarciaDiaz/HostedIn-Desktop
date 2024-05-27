using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HostedInDesktop.Data.Models
{

    using Newtonsoft.Json;
    using System.Collections.Generic;

    public class RootObject
    {
        [JsonProperty("results")]
        public List<Place> Results { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }

    public class Place
    {
        [JsonProperty("formatted_address")]
        public string FormattedAddress { get; set; }

        [JsonProperty("geometry")]
        public Geometry Geometry { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class Geometry
    {
        [JsonProperty("location")]
        public LocationGoogle Location { get; set; }
    }

    public class LocationGoogle
    {
        [JsonProperty("lat")]
        public double Latitude { get; set; }

        [JsonProperty("lng")]
        public double Longitude { get; set; }
    }


}
