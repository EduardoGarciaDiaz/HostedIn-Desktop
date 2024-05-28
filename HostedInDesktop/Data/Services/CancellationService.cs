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
using System.Threading.Tasks;

namespace HostedInDesktop.Data.Services
{
    public class CancellationService : ICancellationService
    {
        public async Task<Cancellation> cancelBooking(Cancellation cancellation)
        {
            try
            {
                var httpClient = APIClient.GetHttpClient();
                var json = JsonConvert.SerializeObject(cancellation);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await httpClient.PostAsync("cancellations/", content);
                if (response.IsSuccessStatusCode)
                {
                    CancellationResponse cancellationResponse = await response.Content.ReadFromJsonAsync<CancellationResponse>();
                    if (response.Headers.TryGetValues("Set-Authorization", out IEnumerable<string> values))
                    {
                        string authorizationHeaderValue = values.FirstOrDefault();
                        // Hacer algo con el valor del encabezado de autorización
                        Console.WriteLine($"Authorization Header: {authorizationHeaderValue}");
                        App.token = authorizationHeaderValue.Substring("Bearer ".Length).Trim();
                    }
                    return await Task.FromResult(cancellationResponse.cancellation);
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
