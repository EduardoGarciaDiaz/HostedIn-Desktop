using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostedInDesktop.Utils
{
    public class MapService : IMapService
    {
        private readonly IConfiguration _configuration;

        public MapService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetMapServiceToken()
        {
            return _configuration["MapServiceToken"];
        }
    }
}
