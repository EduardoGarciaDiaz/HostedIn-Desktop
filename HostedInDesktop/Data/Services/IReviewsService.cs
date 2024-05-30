using HostedInDesktop.Data.Models;
using HostedInDesktop.Data.Services.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostedInDesktop.Data.Services
{
    public interface IReviewsService
    {
        Task<ReviewResponse> CreateAccommodationBookingReview(Review review);
        Task<List<Review>> GetReviewsOfAccommodation(string accommodationId);
    }
}
