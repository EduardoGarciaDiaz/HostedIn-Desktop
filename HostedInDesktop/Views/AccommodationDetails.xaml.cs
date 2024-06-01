using HostedInDesktop.Helper;
using HostedInDesktop.viewmodels;
using Mapsui.Projections;
using Mapsui.Tiling;
using Mapsui.UI.Maui;
using System.ComponentModel;

namespace HostedInDesktop.Views;

public partial class AccommodationDetails : ContentPage
{
    AccommodationDetailsViewModel accommodationDetailsViewModel;
    Pin _pin;

	public AccommodationDetails()
	{
		InitializeComponent();
		BindingContext = ServiceHelper.GetService<AccommodationDetailsViewModel>();
        accommodationDetailsViewModel = (AccommodationDetailsViewModel)BindingContext;

        accommodationDetailsViewModel.PropertyChanged += OnViewModelPropertyChanged;
    }

    private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (sender.Equals(accommodationDetailsViewModel) 
            && accommodationDetailsViewModel.AccommodationData != null)
        {
            InitializeMap();
        }
    }

    private void InitializeMap()
    {
        var map = new Mapsui.Map();

        var tileLayer = OpenStreetMap.CreateTileLayer();
        map.Layers.Add(tileLayer);

        double latitude = accommodationDetailsViewModel.AccommodationData.location.coordinates[1];
        double longitude = accommodationDetailsViewModel.AccommodationData.location.coordinates[0];
        string address = accommodationDetailsViewModel.AccommodationData.location.address.ToString();

        var sphericalMercatorCoordinate = SphericalMercator.FromLonLat(longitude, latitude);
        var centerPoint = new Mapsui.MPoint(sphericalMercatorCoordinate.x, sphericalMercatorCoordinate.y);
        map.Home = n => n.CenterOn(centerPoint);

        mapView.Map = map;

        _pin = new Pin()
        {
            Position = new Position(sphericalMercatorCoordinate.x, sphericalMercatorCoordinate.y),
            Type = PinType.Pin,
            Label = "Hosted In",
            Address = address,
        };

        mapView.Pins.Clear();

        mapView.Pins.Add(_pin);

        _pin.Position = new Position(latitude, longitude);
        mapView.Refresh();

        mapView.Refresh();
    }

    //protected override void OnDisappearing()
    //{
    //    base.OnDisappearing();

    //    accommodationDetailsViewModel.PropertyChanged -= OnViewModelPropertyChanged;
    //}

}