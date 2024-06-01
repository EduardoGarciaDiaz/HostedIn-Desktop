using HostedInDesktop.Data.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HostedInDesktop.Data.Services
{
    public class PlacesClient : IPlacesClient
    {
        public async Task<List<Place>> GetPlaces(string query)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                var url = $"https://maps.googleapis.com/maps/api/place/textsearch/json?query={query}&key={App.Google_API_Keys}";
                string requestBody = $@"
                {{
                    ""textQuery"": ""{query}"",
                    ""pageSize"": 10
                }}";
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Add("X-Goog-Api-Key", App.Google_API_Keys);
                httpClient.DefaultRequestHeaders.Add("X-Goog-FieldMask", "places.displayName,places.formattedAddress,places.location");
                HttpContent content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await httpClient.PostAsync(url, content);
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    var rootObject = JsonConvert.DeserializeObject<RootObject>(json);

                    if (rootObject != null && rootObject.Results != null)
                    {

                        var places = new List<Place>();
                        foreach (var place in rootObject.Results)
                        {
                                                        
                            places.Add(new Place
                            {
                                Name = place.Name,
                                FormattedAddress = place.FormattedAddress,
                                Geometry = place.Geometry
                            });

                        }
                        return places;
                    }
                    else
                    {
                        throw new Exception("Error en la solicitud");
                    }

                }
                else
                {
                    throw new Exception("Error en la solicitud");
                }
                
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<string> GetAddressFromCoordinates(double lat, double lon)
        {
            try
            {
                var latitude = lat; 
                var longitude = lon; 

                var geocodingUrl = $"https://maps.googleapis.com/maps/api/geocode/json?latlng={latitude},{longitude}&key={App.Google_API_Keys}";

                using (HttpClient httpClient = new HttpClient())
                {
                    HttpResponseMessage response = await httpClient.GetAsync(geocodingUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResponse = await response.Content.ReadAsStringAsync();
                        dynamic geocodingResult = JsonConvert.DeserializeObject(jsonResponse);

                        string address = geocodingResult.results[0].formatted_address;

                        return address;
                    }
                    else
                    {
                        throw new Exception("Error al obtener la dirección desde las coordenadas.");
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
