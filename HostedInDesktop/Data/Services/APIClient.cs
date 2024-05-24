namespace HostedInDesktop.Data.Services
{
    public class APIClient
    {
        private static HttpClient HTTP_CLIENT ;

        public static HttpClient GetHttpClient()
        {
            if (HTTP_CLIENT == null)
            {
                HTTP_CLIENT = new HttpClient();
                HTTP_CLIENT.BaseAddress = new Uri("http://localhost:3000/api/v1/");

            }
            return HTTP_CLIENT;
        }
    }
}
