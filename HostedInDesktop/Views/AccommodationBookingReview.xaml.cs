using HostedInDesktop.viewmodels;
using HostedInDesktop.viewmodels.ModelObservable;

namespace HostedInDesktop.Views;

public partial class AccommodationBookingReview : ContentPage
{
	public AccommodationBookingReview()
	{
		InitializeComponent();

        
    }

    private void OnRatingValueChanged(object sender, ValueChangedEventArgs e)
    {
        //if (BindingContext is AccommodationBookingReviewViewModel viewModel)
        //{
        //    viewModel.Rating = (float)e.NewValue;
        //}
    }
}