using HostedInDesktop.Data.JsonConverters;
using HostedInDesktop.Data.Models;
using HostedInDesktop.Data.Services.Responses;
using HostedInDesktop.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


namespace HostedInDesktop.Data.Services;

public class BookingService : IBookingService
{
    public async Task<List<Booking>> GetBookingsByAccommodationId(string accommodationId)
    {
        try
        {
            var httpClient = APIClient.GetHttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", App.token);
            string url = $"accommodations/{accommodationId}/bookings";
            HttpResponseMessage response = await httpClient.GetAsync(url);
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

    public async Task<List<Booking>> GetGuestBookings(string userId, string status)
    {
        try
        {
            var httpClient = APIClient.GetHttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", App.token);
            string url = $"users/{userId}/bookings?status={status}";
            HttpResponseMessage response = await httpClient.GetAsync(url);
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

    public async Task<Booking> CreateBooking(Booking booking)
    {
        try
        {
            var httpClient = APIClient.GetHttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", App.token);

            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            var json = JsonConvert.SerializeObject(booking, settings);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await httpClient.PostAsync("bookings/", content);

            if (response.IsSuccessStatusCode)
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    Converters = { new ProfilePhotoConverter() }
                };

                BookingResponse bookingResponse = await response.Content.ReadFromJsonAsync<BookingResponse>(options);

                return await Task.FromResult(bookingResponse.booking);
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
