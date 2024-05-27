using HostedInDesktop.Data.Models;

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
            if(booking.accommodation.mainImage != null)
            {
                view.imgAccommodation.Source = ImageSource.FromStream(() => new MemoryStream(booking.accommodation.mainImage));

            }
            view.lblTitle.Text = booking.accommodation.title;
            view.lblDescription.Text = booking.accommodation.description;
            view.lblTotalCost.Text = "$" + booking.totalCost.ToString();
            view.lblDates.Text = $"{booking.beginningDate} - {booking.endingDate}";
        }
    }
}