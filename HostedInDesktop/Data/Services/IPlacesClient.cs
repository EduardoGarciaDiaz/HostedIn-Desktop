﻿using HostedInDesktop.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostedInDesktop.Data.Services
{
    public interface IPlacesClient
    {
        Task<List<Place>> GetPlaces(string query);

        Task<string> GetAddressFromCoordinates(double lat, double lon);
    }
}
