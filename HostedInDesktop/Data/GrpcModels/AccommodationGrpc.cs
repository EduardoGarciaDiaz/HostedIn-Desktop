using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostedInDesktop.Data.GrpcModels
{
    public class AccommodationGrpc
    {
        public String Title { get; set; } = "";
        public int BookingsNumber { get; set; } = 0;
        public double Rate { get; set; } = 0;
    }
}
