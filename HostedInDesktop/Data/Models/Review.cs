using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostedInDesktop.Data.Models
{
    public class Review
    {
        public string _id { get; set; }
        public string accommodation { get; set; }
        public string reviewDescription { get; set; }
        public float rating { get; set; }
        public User guestUser { get; set; }
    }
}
