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
    public class ReviewsService : IReviewsService
    {
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
