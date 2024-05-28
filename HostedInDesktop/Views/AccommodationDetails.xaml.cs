using HostedInDesktop.Helper;
using HostedInDesktop.viewmodels;

namespace HostedInDesktop.Views;

public partial class AccommodationDetails : ContentPage
{
	public AccommodationDetails()
	{
		InitializeComponent();
		BindingContext = ServiceHelper.GetService<AccommodationDetailsViewModel>();
	}


}