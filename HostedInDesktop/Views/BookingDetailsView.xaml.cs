using HostedInDesktop.Helper;
using HostedInDesktop.viewmodels;

namespace HostedInDesktop.Views;

public partial class BookingDetailsView : ContentPage
{
	public BookingDetailsView()
	{
		InitializeComponent();
        BindingContext = ServiceHelper.GetService<BookingDetailsViewModel>();
		if (App.hostMode)
		{
			LblQuantity.Text = "¿Cuántos vienen?";
			LblCost.Text = "¿Cuanto vas a recibir?";
			LblUser.Text = "¿Quien va a ser tu invitado?";
		}
		
    }

}