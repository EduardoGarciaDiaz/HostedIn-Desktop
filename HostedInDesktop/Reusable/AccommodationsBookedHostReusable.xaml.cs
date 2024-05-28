using CommunityToolkit.Mvvm.ComponentModel;
using HostedInDesktop.Data.Models;
using HostedInDesktop.viewmodels;
using Microsoft.Maui.Controls;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace HostedInDesktop.Reusable;

public partial class AccommodationsBookedHostReusable : ContentView
{
	public static readonly BindableProperty AccommodationProperty = BindableProperty.Create(nameof(Accommodation), typeof(AccommodationBookingsViewModel), 
																	typeof(AccommodationsBookedHostReusable),null, propertyChanged: OnAccommodationChanged);

    public AccommodationsBookedHostReusable()
    {
        InitializeComponent();
    }
       
    public AccommodationBookingsViewModel Accommodation
    {
        get => (AccommodationBookingsViewModel)GetValue(AccommodationProperty);
        set => SetValue(AccommodationProperty, value);
    }


    private static void OnAccommodationChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var view = (AccommodationsBookedHostReusable)bindable;
        if (newValue is AccommodationBookingsViewModel accommodation)
        {
            accommodation.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(Accommodation.Accommodation.mainImage))
                {
                    UpdateImage(view, accommodation.Accommodation);
                }
            };
            UpdateImage(view, accommodation.Accommodation  );
            view.lblTitle.Text = accommodation.Accommodation.title;
            view.lblDescription.Text = accommodation.Accommodation.description;
            view.lblPrice.Text = $"${accommodation.Accommodation.nightPrice} por noche";

        }

    }

    private static void UpdateImage(AccommodationsBookedHostReusable view, Accommodation accommodation)
    {
        if (accommodation.mainImage != null && accommodation.mainImage.Length > 0)
        {
            view.imgAccommodation.Source = ImageSource.FromStream(() => new MemoryStream(accommodation.mainImage));
        }
        else
        {
            view.imgAccommodation.Source = ImageSource.FromFile("img_provisional.png");
        }
    }

}