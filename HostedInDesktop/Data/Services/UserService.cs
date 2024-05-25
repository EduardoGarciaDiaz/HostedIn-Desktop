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
    public class UserService : IUserService
    {

        public async Task<User> EditAccount(string userId, User userToEdit)
        {
            try
            {
                var httpClient = APIClient.GetHttpClient();
                var path = $"users/{userId}";


                var json = JsonConvert.SerializeObject(userToEdit);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await httpClient.PutAsync(path, content);

                if (response.IsSuccessStatusCode)
                {
                    UserResponse userResponse = await response.Content.ReadFromJsonAsync<UserResponse>();
                    return await Task.FromResult(userResponse.User);
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

        public async Task<string> DeleteAccount(string userId)
        {
            try
            {
                var httpClient = APIClient.GetHttpClient();

                var path = $"users/{userId}";

                HttpResponseMessage response = await httpClient.DeleteAsync(path);

                if (response.IsSuccessStatusCode)
                {
                    UserResponse userResponse = await response.Content.ReadFromJsonAsync<UserResponse>();
                    return await Task.FromResult(userResponse.User._id);
                }
                else
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    JObject jsonObject = JObject.Parse(jsonResponse);
                    string errorMessage = (string)jsonObject["message"];
                    throw new ApiException(errorMessage);
                }

            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
