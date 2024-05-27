using HostedInDesktop.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostedInDesktop.Data.Services
{
    public interface IAccommodationsService
    {
        Task<List<Accommodation>> GetAccommodationsAsync(string id);
        Task<List<Accommodation>> GetHostBookedAccommodationAsync(string id);
        Task<List<Accommodation>> GetHostOwnedAccommodationsAsync(string userId);
    }
}
