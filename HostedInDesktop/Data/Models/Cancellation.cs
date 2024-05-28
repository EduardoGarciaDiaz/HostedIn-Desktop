using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostedInDesktop.Data.Models
{
    public class Cancellation
    {
        public string _id {  get; set; }
        public string reason { get; set; }
        public DateTime cancellationDate { get; set; }
        public Booking booking { get; set; }
        public User cancellator { get; set; }
    }
}
