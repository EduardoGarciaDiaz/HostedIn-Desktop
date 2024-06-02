namespace HostedInDesktop.Data.Services
{
    public class APIClient
    {
        private static HttpClient HTTP_CLIENT;

        public static HttpClient GetHttpClient()
        {
            if (HTTP_CLIENT == null)
            {
                HTTP_CLIENT = new HttpClient();
                HTTP_CLIENT.BaseAddress = new Uri("http://192.168.56.106/HostedIn-Server/api/v1/");
            }
            return HTTP_CLIENT;
        }

    }
}
