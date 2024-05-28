using HostedInDesktop.Data.Models;
using HostedInDesktop.viewmodels;

namespace HostedInDesktop.Views;

public partial class EditAccommodation : ContentView
{
	public EditAccommodation(EditAccommodationViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}