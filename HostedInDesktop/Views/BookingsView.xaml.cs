using HostedInDesktop.Helper;
using HostedInDesktop.viewmodels;

namespace HostedInDesktop.Views;

public partial class BookingsView : ContentView
{
	public BookingsView()
	{
		InitializeComponent();
        BindingContext = ServiceHelper.GetService<BookingsGuestViewViewModel>();
    }
}