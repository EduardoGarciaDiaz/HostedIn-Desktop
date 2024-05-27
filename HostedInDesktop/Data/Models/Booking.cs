using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostedInDesktop.Data.Models
{
    public class Booking
    {
        public string _id { get; set; }
        public Accommodation accommodation { get; set; }
        public string beginningDate { get; set; }
        public string endingDate { get; set; }
        public int numberOfGuests { get; set; }
        public string bookingStatus { get; set; }
        public double totalCost { get; set; }
        public User hostUser { get; set; }
        public User guestUser { get; set; }
    }

    public enum BookingStatus
    {
        CURRENT,
        OVERDUE
    }

    public static class BookingStatusExtensions
    {
        private static readonly Dictionary<BookingStatus, string> StatusDescriptions = new Dictionary<BookingStatus, string>
        {
            { BookingStatus.CURRENT, "Current"},
            {BookingStatus.OVERDUE, "Overdue" }   
        };

        public static string GetDescription(this BookingStatus status)
        {
            return StatusDescriptions[status];
        }
    }
}
