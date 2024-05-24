using HostedInDesktop.Data.Models;
using HostedInDesktop.Data.Services.Responses;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace HostedInDesktop.Data.Services
{
    
    public class HostedInService
    {

        HttpClient _httpClient = APIClient.GetHttpClient();
       
        public interface IAuthCallback
        {
            void OnSuccess(User user);
            void OnError(string errorMessage);
        }


        public async Task LoginAsync(User user, IAuthCallback authcallback)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync("auth/signin", user);
                if (response.IsSuccessStatusCode)
                {
                    SigninResponse signinResponse = await response.Content.ReadFromJsonAsync<SigninResponse>();
                    if (signinResponse != null)
                    {
                        //Hacer algo con el token
                        string token = response.Headers.GetValues("Authorization").FirstOrDefault();
                        authcallback.OnSuccess(signinResponse.user);
                    }
                    else
                    {
                        authcallback.OnError("Error al procesar la solicitud");
                    }
                } 
                else
                {
                    string errorString = await response.Content.ReadAsStringAsync();
                    JsonDocument json = JsonDocument.Parse(errorString);
                    string message = json.RootElement.GetProperty("message").GetString();
                    authcallback.OnError(message);
                }
            }catch (Exception ex)
            {
                authcallback.OnError(ex.Message);  
            }

        }
    }
}
