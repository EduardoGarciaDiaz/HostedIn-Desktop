using HostedInDesktop.Helper;
using HostedInDesktop.viewmodels;

namespace HostedInDesktop.Views;

public partial class BookingDetailsView : ContentPage
{
	public BookingDetailsView()
	{
		InitializeComponent();
        BindingContext = ServiceHelper.GetService<BookingDetailsViewModel>();
    }

}