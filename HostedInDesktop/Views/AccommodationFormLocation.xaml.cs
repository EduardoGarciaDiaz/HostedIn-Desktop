using HostedInDesktop.Data.Models;
using HostedInDesktop.viewmodels;
using Mapsui.Projections;
using Mapsui.Widgets.Zoom;
using Mapsui;
using System.Diagnostics;
using Mapsui.Extensions;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Extensions.Configuration;
using HostedInDesktop.Utils;


namespace HostedInDesktop.Views;

public partial class AccommodationFormLocation : ContentView
{
    private readonly AccommodationFormViewModel _viewModel;
    private readonly EditAccommodationFormViewModel _editViewModel;
    private readonly IMapService _mapService;
    private string _mapServiceToken;

    public Microsoft.Maui.Devices.Sensors.Location location;

    public AccommodationFormLocation(AccommodationFormViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;

        var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

        var configuration = builder.Build();
        _mapServiceToken = configuration["MapService:Token"];

        Location location = new Location
        {
            latitude = Double.Parse(lblLatitude.Text.ToString()),
            longitude = Double.Parse(lblLongitude.Text.ToString()),
            address = lblAddress.Text.ToString()
        };

        _viewModel.SelectedLocation = location;
    }

    public AccommodationFormLocation(EditAccommodationFormViewModel viewModel, Accommodation accommodation)
    {
        InitializeComponent();
        _editViewModel = viewModel;
        BindingContext = _viewModel;

        var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);


        var configuration = builder.Build();
        _mapServiceToken = configuration["MapService:Token"];

        lblLatitude.Text = accommodation.location.latitude.ToString();
        lblLongitude.Text = accommodation.location.longitude.ToString();

        Location location = new Location
        {
            latitude = Double.Parse(lblLatitude.Text.ToString()),
            longitude = Double.Parse(lblLongitude.Text.ToString()),
            address = accommodation.location.address.ToString()
        };

        _editViewModel.SelectedLocation = location;
    }

    private async void OnSearchLocationClicked(object sender, EventArgs e)
    {
        string placeName = etx_location_search.Text;

        //ShowPlaceOnMap(placeName);
    }

    //private void MapView_MapClicked(object sender, MapClickedEventArgs e)
    //{
    //    // Obtener las coordenadas de la ubicación seleccionada
    //    double latitude = e.Location.Latitude;
    //    double longitude = e.Location.Longitude;

    //    // Convertir las coordenadas a una dirección (puedes usar servicios de geocodificación)
    //    string address = GetAddressFromCoordinates(latitude, longitude);

    //    // Actualizar la propiedad SelectedLocation del ViewModel

    //    Data.Models.Location location = new Data.Models.Location
    //    {
    //        latitude = latitude,
    //        longitude = longitude,
    //        address = address
    //    };

    //    _viewModel.SelectedLocation = location;
    //}

    private string GetAddressFromCoordinates(double latitude, double longitude)
    {
        // Lógica para obtener la dirección desde las coordenadas
        // Por ejemplo, usando servicios de geocodificación como Google Maps Geocoding API
        return "Hosted In";
    }
}