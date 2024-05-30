using GoogleApi.Entities.Maps.Directions.Response;
using GoogleApi.Entities.Search.Video.Common;
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

public class AccommodationsService : IAccommodationsService
{
    public async Task<List<Accommodation>> GetAccommodationsAsync(string id)
    {
        try
        {
            var httpClient = APIClient.GetHttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", App.token);
            string url = $"accommodations?id={id}";
            HttpResponseMessage response = await httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                GetAccommodationsResponse getAccommodationsResponse = JsonConvert.DeserializeObject<GetAccommodationsResponse>(jsonResponse);
                if (getAccommodationsResponse != null && getAccommodationsResponse.Accommodations != null)
                {
                    return await Task.FromResult(getAccommodationsResponse.Accommodations);
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

    public async Task<List<Accommodation>> GetHostBookedAccommodationAsync(string id)
    {
        try
        {
            var httpClient = APIClient.GetHttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", App.token);
            string url = $"users/{id}/accommodations?atLeastOneBooking={true}";
            HttpResponseMessage response = await httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                GetAccommodationsResponse getAccommodationsResponse = JsonConvert.DeserializeObject<GetAccommodationsResponse>(jsonResponse);
                if (getAccommodationsResponse != null && getAccommodationsResponse.Accommodations != null)
                {
                    return await Task.FromResult(getAccommodationsResponse.Accommodations);
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

    public async Task<List<Accommodation>> GetHostOwnedAccommodationsAsync(string userId)
    {
        try
        {
            var httpClient = APIClient.GetHttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", App.token);
            string url = $"users/{userId}/accommodations";
            HttpResponseMessage response = await httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                GetAccommodationsResponse getAccommodationsResponse = JsonConvert.DeserializeObject<GetAccommodationsResponse>(jsonResponse);
                if (getAccommodationsResponse != null && getAccommodationsResponse.Accommodations != null)
                {
                    return await Task.FromResult(getAccommodationsResponse.Accommodations);
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

        public async Task<List<Accommodation>> GetAccommodationsAsync(string id, double lat, double lng) {
            try
            {
                var httpClient = APIClient.GetHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", App.token);

                string url = $"accommodations?id={id}&lat={lat}&long={lng}";
                HttpResponseMessage response = await httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    GetAccommodationsResponse getAccommodationsResponse = JsonConvert.DeserializeObject<GetAccommodationsResponse>(jsonResponse);
                    if (getAccommodationsResponse != null && getAccommodationsResponse.Accommodations != null)
                    {
                        return await Task.FromResult(getAccommodationsResponse.Accommodations);
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


        public async Task<Accommodation> CreateAccommodationAsync(Accommodation accommodation)
        {
            try
            {
                var httpClient = APIClient.GetHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", App.token);

                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                };

                var json = JsonConvert.SerializeObject(accommodation, settings);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await httpClient.PostAsync("accommodations/", content);

                if (response.IsSuccessStatusCode)
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                        Converters = { new ProfilePhotoConverter() }
                    };

                    AccommodationResponse accommodationResponse = await response.Content.ReadFromJsonAsync<AccommodationResponse>(options);
                    
                    return await Task.FromResult(accommodationResponse.Accommodation);
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

    public async Task<Accommodation> UpdateAccommodation(Accommodation accommodation)
    {
        try
        {
            var httpClient = APIClient.GetHttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", App.token);
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };
            var json = JsonConvert.SerializeObject(accommodation, settings);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            string url = $"accommodations/{accommodation._id}/";
            HttpResponseMessage response = await httpClient.PutAsync(url, content);
            if (response.IsSuccessStatusCode)
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    Converters = { new ProfilePhotoConverter() }
                };

                AccommodationResponse accommodationResponse = await response.Content.ReadFromJsonAsync<AccommodationResponse>(options);
                return await Task.FromResult(accommodationResponse.Accommodation);
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

    public async Task<string> DeleteAccommodation(string accommodationId)
    {
        try
        {

            var httpClient = APIClient.GetHttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", App.token);
            string url = $"accommodations/{accommodationId}/";
            HttpResponseMessage response = await httpClient.DeleteAsync(url);
            if (response.IsSuccessStatusCode)
            {

                DeleteAccommodationResponse accommodationResponse = await response.Content.ReadFromJsonAsync<DeleteAccommodationResponse>();
                return await Task.FromResult(accommodationResponse.Message);
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

