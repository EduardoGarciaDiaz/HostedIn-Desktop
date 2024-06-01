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
using Mapsui.Tiling;
using Mapsui.UI.Maui;
using HostedInDesktop.Data.Services;


namespace HostedInDesktop.Views;

public partial class AccommodationFormLocation : ContentView
{
    private readonly AccommodationFormViewModel _viewModel;
    private readonly EditAccommodationFormViewModel _editViewModel;
    private readonly IPlacesClient _placesClient = new PlacesClient();

    private Pin _pin;

    public Microsoft.Maui.Devices.Sensors.Location location;

    public AccommodationFormLocation(AccommodationFormViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;

        InitializeMap();

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
        BindingContext = _editViewModel;

        InitializeMap();

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

    private void InitializeMap()
    {
        var map = new Mapsui.Map();

        var tileLayer = OpenStreetMap.CreateTileLayer();
        map.Layers.Add(tileLayer);

        double latitude = Double.Parse(lblLatitude.Text);
        double longitude = Double.Parse(lblLongitude.Text);

        var sphericalMercatorCoordinate = SphericalMercator.FromLonLat(longitude, latitude);
        var centerPoint = new Mapsui.MPoint(sphericalMercatorCoordinate.x, sphericalMercatorCoordinate.y);
        map.Home = n => n.CenterOn(centerPoint);           

        mapView.Map = map;

        _pin = new Pin()
        {
            Position = new Position(sphericalMercatorCoordinate.x, sphericalMercatorCoordinate.y),
            Type = PinType.Pin,
            Label = "Hosted In",
            Address = "Hosted In",
        };

        mapView.Pins.Add(_pin);
        _pin.Position = new Position(latitude, longitude);

        mapView.MapClicked += OnMapClicked;
        
        mapView.Refresh();
    }

    private async void OnMapClicked(object sender, MapClickedEventArgs e)
    {
        _pin.Position = e.Point;
        double latitude = e.Point.Latitude;
        double longitude = e.Point.Longitude;
        string address = await GetAddressFromCoordinates(e.Point.Latitude, e.Point.Longitude);
        mapView.Refresh();

        var location = new Location
        {
            latitude = latitude,
            longitude = longitude,
            address = address
        };

        lblLatitude.Text = latitude.ToString();
        lblLongitude.Text = longitude.ToString();
        lblAddress.Text = address;

        if (_viewModel != null)
        {
            _viewModel.SelectedLocation = location;
        }
        else if (_editViewModel != null)
        {
            _editViewModel.SelectedLocation = location;
        }
    }

    private async void OnSearchLocationClicked(object sender, EventArgs e)
    {
        //TODO:
    }

    private async Task<string> GetAddressFromCoordinates(double latitude, double longitude)
    {
        string address = "Hosted In";
        address = await _placesClient.GetAddressFromCoordinates(latitude, longitude);

        return address;
    }
}