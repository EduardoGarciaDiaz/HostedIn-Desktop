using HostedInDesktop.Data.Models;
using HostedInDesktop.Data.Services.Responses;
using HostedInDesktop.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace HostedInDesktop.Data.Services
{
    public class BookingService : IBookingService
    {
        public async Task<List<Booking>> GetBookingsByAccommodationId(string accommodationId)
        {
            try
            {
                var httpClient = APIClient.GetHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", App.token);
                string url = $"accommodations/{accommodationId}/bookings";
                HttpResponseMessage response =await httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    GetBookingResponse getBookingResponse = JsonConvert.DeserializeObject<GetBookingResponse>(jsonResponse);
                    if (getBookingResponse != null && getBookingResponse.Bookings != null)
                    {
                        return await Task.FromResult(getBookingResponse.Bookings);
                    }
                    else
                    {
                        throw new ApiException("Servicio no disponible en este momento");
                    }
                }
                else
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    JObject jsonObject = JObject.Parse(jsonResponse);
                    string errorMessage = (string)jsonObject["message"];
                    throw new ApiException(errorMessage);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
