using HostedInDesktop.Data.Models;
using HostedInDesktop.viewmodels;

namespace HostedInDesktop.Views;

public partial class EditAccommodation : ContentView
{
	public EditAccommodation(Accommodation accommodation)
	{
		InitializeComponent();
		if(BindingContext is EditAccommodationViewModel editAccommodationViewModel){			
			editAccommodationViewModel.Accommodation = accommodation;
		}
		
	}
}