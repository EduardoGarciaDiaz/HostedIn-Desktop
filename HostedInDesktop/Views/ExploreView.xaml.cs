using HostedInDesktop.Data.Models;
using HostedInDesktop.Helper;
using HostedInDesktop.viewmodels;

namespace HostedInDesktop.Views;

public partial class ExploreView : ContentView
{
	ExploreViewModel exploreViewModel;
	public ExploreView()
	{
		InitializeComponent();
        BindingContext = ServiceHelper.GetService<ExploreViewModel>();
		exploreViewModel = (ExploreViewModel)BindingContext;

	}

    private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        if (e.Item != null && e.Item is Place selectedPlace)
        {
            exploreViewModel.OnPlaceSelected(selectedPlace);
        }

        ((ListView)sender).SelectedItem = null;
    }

    private async void OnSearchBarTextChanged(object sender, TextChangedEventArgs e)
    {
        if (string.IsNullOrEmpty(e.NewTextValue))
        {
            lvPlaces.IsVisible = false;
            await exploreViewModel.LoadAccommodationsAsync();

        }
    }
}