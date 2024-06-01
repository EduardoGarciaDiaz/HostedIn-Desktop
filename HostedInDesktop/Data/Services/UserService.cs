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
    public class UserService : IUserService
    {

        public async Task<User> GetUserById(string userId)
        {
            try             {
                var httpClient = APIClient.GetHttpClient();
                var path = $"users/{userId}";

                HttpResponseMessage response = await httpClient.GetAsync(path);

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

                    var userResponse = await response.Content.ReadFromJsonAsync<UserResponse>(options);

                    return await Task.FromResult(userResponse.User);
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

        public async Task<User> EditAccount(string userId, User userToEdit)
        {
            try
            {
                var httpClient = APIClient.GetHttpClient();
                var path = $"users/{userId}";

                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                };

                var json = JsonConvert.SerializeObject(userToEdit, settings);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await httpClient.PutAsync(path, content);

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
                    UserResponse userResponse = await response.Content.ReadFromJsonAsync<UserResponse>(options);
                    return await Task.FromResult(userResponse.User);
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

        public async Task<string> DeleteAccount(string userId)
        {
            try
            {
                var httpClient = APIClient.GetHttpClient();

                var path = $"users/{userId}";

                HttpResponseMessage response = await httpClient.DeleteAsync(path);

                if (response.IsSuccessStatusCode)
                {
                    if (response.Headers.TryGetValues("Set-Authorization", out IEnumerable<string> values))
                    {
                        string authorizationHeaderValue = values.FirstOrDefault();
                        App.token = authorizationHeaderValue.Substring("Bearer ".Length).Trim();
                    }
                    DeleteAccountResponse deleteResponse = await response.Content.ReadFromJsonAsync<DeleteAccountResponse>();
                    return await Task.FromResult(deleteResponse.UserId);
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
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<string> SendEmailCode(GenericStringClass email)
        {
            try
            {
                var httpClient = APIClient.GetHttpClient();

                var path = "users/passwords";

                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                };

                var json = JsonConvert.SerializeObject(email, settings);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await httpClient.PostAsync(path, content);

                if (response.IsSuccessStatusCode)
                {
                    RecoverPasswordRepsonse result = await response.Content.ReadFromJsonAsync<RecoverPasswordRepsonse>();
                    return await Task.FromResult(result.message);
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

        public async Task<string> VerifyCode(GenericStringClass code)
        {
            try
            {
                var httpClient = APIClient.GetHttpClient();

                var path = "users/passwords/code";

                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                };

                var json = JsonConvert.SerializeObject(code, settings);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await httpClient.PostAsync(path, content);

                if (response.IsSuccessStatusCode)
                {
                    if (response.Headers.TryGetValues("Authorization", out IEnumerable<string> values))
                    {
                        string authorizationHeaderValue = values.FirstOrDefault();
                        return authorizationHeaderValue.Substring("Bearer ".Length).Trim();
                    }
                    else
                    {
                        return "";
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
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<string> updatePassword(RecoverPassswordRequest recoverPasssword, string token)
        {
            try
            {
                var httpClient = APIClient.GetHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(token);
                var path = "users";

                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                };

                var json = JsonConvert.SerializeObject(recoverPasssword, settings);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await httpClient.PatchAsync(path, content);

                if (response.IsSuccessStatusCode)
                {
                    RecoverPasswordRepsonse result = await response.Content.ReadFromJsonAsync<RecoverPasswordRepsonse>();
                    return await Task.FromResult(result.message);
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
