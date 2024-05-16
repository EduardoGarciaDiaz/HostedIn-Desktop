using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HostedIn_DesktopClient.Models;
using Newtonsoft.Json;

public class ApiRest
{
    private static readonly HttpClient HTTPS_CLIENT = new HttpClient();
    private static ApiRest _instance;

    private ApiRest()
    {
        HTTPS_CLIENT.BaseAddress = new Uri("http://localhost:3000/api/v1/");
    }

    public static ApiRest GetInstance()
    {
        if (_instance == null)
        {
            _instance = new ApiRest();
        }
        return _instance;
    }

    public async Task<HttpResponseMessage> CreateAccount(User newUser)
    {
        var json = JsonConvert.SerializeObject(newUser);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        return await HTTPS_CLIENT.PostAsync("auth/signup", content);
    }
}
