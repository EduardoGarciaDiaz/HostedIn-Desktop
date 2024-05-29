using HostedInDesktop.Helper;
using HostedInDesktop.viewmodels;

namespace HostedInDesktop.Views;

public partial class AccommodationBooking : ContentPage
{
	public AccommodationBooking()
	{
		InitializeComponent();
        BindingContext = ServiceHelper.GetService<AccommodationBookingViewModel>();
    }
}