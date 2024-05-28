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
            accommodation.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(Accommodation.mainImage))
                {
                    UpdateImage(view, accommodation);
                }
            };
            UpdateImage(view, accommodation);
            view.lblTitle.Text = accommodation.title;
            view.lblDescription.Text = accommodation.description;
            view.lblPrice.Text = $"${accommodation.nightPrice} por noche";
        }
    }

    private static void UpdateImage(AccommodationOwnedReusable view, Accommodation accommodation)
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