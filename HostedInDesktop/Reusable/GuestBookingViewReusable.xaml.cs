using HostedInDesktop.Data.Models;
using HostedInDesktop.Utils;

namespace HostedInDesktop.Reusable;

public partial class GuestBookingViewReusable : ContentView
{

	public static readonly BindableProperty BookingProperty = BindableProperty.Create(nameof(Booking), typeof(Booking),
																typeof(GuestBookingViewReusable), null, propertyChanged: OnBookingChaged);

   

    public GuestBookingViewReusable()
	{
		InitializeComponent();
	}

    public Booking Booking 
    {
        get => (Booking)GetValue(BookingProperty); 
        set => SetValue(BookingProperty, value);   
    }

    private static void OnBookingChaged(BindableObject bindable, object oldValue, object newValue)
    {
        var view = (GuestBookingViewReusable)bindable;
        if (newValue is Booking booking)
        {
            booking.accommodation.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(Accommodation.mainImage))
                {
                    UpdateImage(view, booking.accommodation);
                }
            };

            UpdateImage(view, booking.accommodation);
            view.lblTitle.Text = booking.accommodation.title;
            view.lblDescription.Text = booking.accommodation.description;
            view.lblTotalCost.Text = "$" + booking.totalCost.ToString();
            view.lblDates.Text = $"{DateFormatterUtils.ConvertToReadableDate(booking.beginningDate)} - {DateFormatterUtils.ConvertToReadableDate(booking.endingDate)}";
        }
    }

    private static void UpdateImage(GuestBookingViewReusable view, Accommodation accommodation)
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