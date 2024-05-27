using HostedInDesktop.viewmodels;

namespace HostedInDesktop.Views;

public partial class BookingsView : ContentView
{
	public BookingsView(BookingsGuestViewViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}