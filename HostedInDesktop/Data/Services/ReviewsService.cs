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

namespace HostedInDesktop.Data.Services
{
    public class ReviewsService : IReviewsService
    {
        public async Task<ReviewResponse> CreateAccommodationBookingReview(Review review)
        {
            try
            {
                var httpClient = APIClient.GetHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", App.token);
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                };
                var json = JsonConvert.SerializeObject(review, settings);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                string url = "reviews";
                HttpResponseMessage response = await httpClient.PostAsync(url, content);
                if (response.IsSuccessStatusCode)
                {
                    if (response.Headers.TryGetValues("Set-Authorization", out IEnumerable<string> values))
                    {
                        string authorizationHeaderValue = values.FirstOrDefault();
                        App.token = authorizationHeaderValue.Substring("Bearer ".Length).Trim();
                    }
                    var options = new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                        Converters = { new ProfilePhotoConverter() }
                    };
                    ReviewResponse reviewResponse  = await response.Content.ReadFromJsonAsync<ReviewResponse>(options);
                    return await Task.FromResult(reviewResponse);
                }
                else
                {

                    if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        throw new UnauthorizedAccessException("Acceso no autorizado. Por favor, vuelve a iniciar sesion");
                    }
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

        public async Task<List<Review>> GetReviewsOfAccommodation(string accommodationId)
        {
            try
            {
                var httpClient = APIClient.GetHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", App.token);
                string path = $"accommodations/{accommodationId}/reviews";
                HttpResponseMessage response = await httpClient.GetAsync(path);

                if (response.IsSuccessStatusCode)
                {
                    if (response.Headers.TryGetValues("Set-Authorization", out IEnumerable<string> values))
                    {
                        string authorizationHeaderValue = values.FirstOrDefault();
                        App.token = authorizationHeaderValue.Substring("Bearer ".Length).Trim();
                    }
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    GetReviewsResponse getReviewsResponse = JsonConvert.DeserializeObject<GetReviewsResponse>(jsonResponse);
                    if (getReviewsResponse != null && getReviewsResponse.Reviews != null)
                    {
                        return await Task.FromResult(getReviewsResponse.Reviews);
                    }
                    else
                    {
                        throw new ApiException("Servicio no disponible en este momento");
                    }
                }
                else
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        throw new UnauthorizedAccessException("Acceso no autorizado. Por favor, vuelve a iniciar sesion");
                    }
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
