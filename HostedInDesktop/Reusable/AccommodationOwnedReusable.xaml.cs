using CommunityToolkit.Mvvm.Input;
using HostedInDesktop.Data.Models;
namespace HostedInDesktop.Reusable;

public partial class AccommodationOwnedReusable : ContentView
{


    public static readonly BindableProperty AccommodationProperty = BindableProperty.Create(nameof(Accommodation),typeof(Accommodation),
                                                                    typeof(AccommodationExploreReusable), null, propertyChanged: OnAccommodationChanged);

    public AccommodationOwnedReusable()
	{
        InitializeComponent();
	}

    public Accommodation Accommodation
    {
        get => (Accommodation)GetValue(AccommodationProperty);
        set => SetValue(AccommodationProperty, value);
    }


    private static void OnAccommodationChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var view = (AccommodationOwnedReusable)bindable;
        if (newValue is Accommodation accommodation)
        {
            if (accommodation.mainImage != null)
            {
                view.imgAccommodation.Source = ImageSource.FromStream(() => new MemoryStream(accommodation.mainImage));
            }
            view.lblTitle.Text = accommodation.title;
            view.lblDescription.Text = accommodation.description;
            view.lblPrice.Text = $"${accommodation.nightPrice} por noche";
        }
    }

    
}