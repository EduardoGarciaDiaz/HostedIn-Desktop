using HostedInDesktop.Data.Models;
using HostedInDesktop.Utils;

namespace HostedInDesktop.Reusable;

public partial class BookingMainInfoReusable : ContentView
{
    public static readonly BindableProperty BookingProperty = BindableProperty.Create(
        nameof(Booking), typeof(Booking), typeof(BookingMainInfoReusable), propertyChanged: OnBookedChanged);

    public BookingMainInfoReusable()
    {
        InitializeComponent();
    }

    public Booking Booking
    {
        get => (Booking)GetValue(BookingProperty);
        set => SetValue(BookingProperty, value);
    }

    private static void OnBookedChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var view = (BookingMainInfoReusable)bindable;
        if (newValue is Booking booking)
        {
            view.lblguestName.Text = booking.guestUser.fullName;
            view.lblDates.Text = DateFormatterUtils.ConvertToReadableDate(booking.beginningDate.ToString()) + " - " + DateFormatterUtils.ConvertToReadableDate(booking.endingDate.ToString());
            view.lblStatus.Text = TranslatorToSpanish.TranslateBookingStatusValue(booking.bookingStatus);
        }
    }
}

