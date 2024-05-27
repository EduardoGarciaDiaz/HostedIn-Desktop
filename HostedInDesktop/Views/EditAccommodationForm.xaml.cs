using HostedInDesktop.Data.Models;
using HostedInDesktop.viewmodels;
using Newtonsoft.Json;

namespace HostedInDesktop.Views;

public partial class EditAccommodationForm : ContentPage
{  
    public EditAccommodationForm(EditAccommodationFormViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

}