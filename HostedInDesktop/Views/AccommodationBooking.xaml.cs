using HostedInDesktop.Helper;
using HostedInDesktop.viewmodels;
using Syncfusion.Maui.Calendar;

namespace HostedInDesktop.Views;

public partial class AccommodationBooking : ContentPage
{
	public AccommodationBooking()
	{
		InitializeComponent();
        BindingContext = ServiceHelper.GetService<AccommodationBookingViewModel>();
    }

    private void OnSelectionChanged(object sender, CalendarSelectionChangedEventArgs e)
    {
        var viewModel = BindingContext as AccommodationBookingViewModel;
        if (viewModel != null && e.NewValue is CalendarDateRange range)
        {
            viewModel.SelectedBookingDates = range;
            viewModel.SelectBookingDates();
        }
    }
}