using HostedInDesktop.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostedInDesktop.Data.Services
{
    public interface IBookingService
    {
        Task<List<Booking>> GetBookingsByAccommodationId(String accommodationId);
        Task<List<Booking>> GetGuestBookings(String userId, String status);
        Task<Booking> CreateBooking(Booking booking);
    }
}
