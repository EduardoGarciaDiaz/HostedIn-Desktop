using HostedInDesktop.viewmodels;

namespace HostedInDesktop.Views;

public partial class HostBookedAccommodations : ContentView
{
	public HostBookedAccommodations(AcoommodationsBookedHostViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}