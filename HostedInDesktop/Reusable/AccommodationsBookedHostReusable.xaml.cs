using CommunityToolkit.Mvvm.ComponentModel;
using HostedInDesktop.Data.Models;
using HostedInDesktop.viewmodels;
using Microsoft.Maui.Controls;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace HostedInDesktop.Reusable;

public partial class AccommodationsBookedHostReusable : ContentView
{
	public static readonly BindableProperty AccommodationProperty = BindableProperty.Create(nameof(Accommodation), typeof(Accommodation), 
																	typeof(AccommodationsBookedHostReusable),null);

    public AccommodationsBookedHostReusable()
    {
        InitializeComponent();
    }
       
    public Accommodation Accommodation
    {
        get => (Accommodation)GetValue(AccommodationProperty);
        set => SetValue(AccommodationProperty, value);
    }


    //private static void OnAccommodationChanged(BindableObject bindable, object oldValue, object newValue)
    //{
    //    var view = (AccommodationsBookedHostReusable)bindable;
    //    if (newValue is Accommodation accommodation)
    //    {
    //        if (accommodation.mainImage != null)
    //        {
    //            view.imgAccommodation.Source = ImageSource.FromStream(() => new MemoryStream(accommodation.mainImage));
    //        }
    //        view.lblTitle.Text = accommodation.title;
    //        view.lblDescription.Text = accommodation.description;
    //        view.lblPrice.Text = $"${accommodation.nightPrice} por noche";
            
    //    }
        
    //}

}