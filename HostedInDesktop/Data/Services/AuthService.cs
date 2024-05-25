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
    public class AuthService : IAuthService
    {

        public async Task<User> SignIn(string email, string password)
        {
            try
            {
                var httpClient = APIClient.GetHttpClient();

                var data = new
                {
                    email,
                    password,
                };

                var json = JsonConvert.SerializeObject(data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await httpClient.PostAsync("auth/signin", content);
                if (response.IsSuccessStatusCode)
                {
                    SigninResponse signinResponse = await response.Content.ReadFromJsonAsync<SigninResponse>();
                    return await Task.FromResult(signinResponse.user);
                } 
                else
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    JObject jsonObject = JObject.Parse(jsonResponse);
                    string errorMessage = (string)jsonObject["message"];
                    throw new ApiException(errorMessage);
                }
            } catch (Exception)
            {
                throw;
            }
        }

        public Task<User> SignUp(User user)
        {
            throw new NotImplementedException();
        }
    }
}
