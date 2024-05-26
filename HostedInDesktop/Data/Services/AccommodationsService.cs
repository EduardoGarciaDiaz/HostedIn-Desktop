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

        public async Task<List<Accommodation>> GetAccommodationsAsync(string id, double lat, double lng)
        {
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
    }
}
