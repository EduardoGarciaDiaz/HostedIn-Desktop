

using HostedInDesktop.viewmodels;

namespace HostedInDesktop.Views;


public partial class HostAccommodationsView : ContentView
{
	public HostAccommodationsView(AccommodationsOwnedViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}