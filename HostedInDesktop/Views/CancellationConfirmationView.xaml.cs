using HostedInDesktop.Helper;
using HostedInDesktop.viewmodels;

namespace HostedInDesktop.Views;

public partial class CancellationConfirmationView : ContentPage
{
	public CancellationConfirmationView()
	{
		InitializeComponent();
		BindingContext = ServiceHelper.GetService<CancellationConfirmationViewModel>();
	}
}