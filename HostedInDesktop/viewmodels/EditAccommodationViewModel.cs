

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HostedInDesktop.Data.Models;
using HostedInDesktop.Views;
using Newtonsoft.Json;

namespace HostedInDesktop.viewmodels;

public partial class EditAccommodationViewModel : ObservableObject
{
    [ObservableProperty]
    Accommodation _accommodation;

    [ObservableProperty]
    private bool isLoading;


    public EditAccommodationViewModel(Accommodation accommodation)
    {
        _accommodation = accommodation;
    }



    [RelayCommand]
    private async Task EditAccommodationTypeAsync(Accommodation accommodation)
    {
        var navegationParameter = new ShellNavigationQueryParameters
        {
            {"Accommodation", _accommodation },
            {"index", 1 }
        };
        await Shell.Current.GoToAsync($"{nameof(EditAccommodationForm)}", navegationParameter);
    }

    [RelayCommand]
    private async Task EditAccommodationUbicationAsync(Accommodation accommodation)
    {
        var navegationParameter = new ShellNavigationQueryParameters
        {
            {"Accommodation", _accommodation },
            {"index", 2 }
        };
        await Shell.Current.GoToAsync($"{nameof(EditAccommodationForm)}", navegationParameter);
    }

    [RelayCommand]
    private async Task EditAccommodationRoomsNumbersAsync(Accommodation accommodation)
    {
        var navegationParameter = new ShellNavigationQueryParameters
        {
            {"Accommodation", _accommodation },
            {"index", 3 }
        };
        await Shell.Current.GoToAsync($"{nameof(EditAccommodationForm)}", navegationParameter);
    }

    [RelayCommand]
    private async Task EditAccommodationServicesAsync(Accommodation accommodation)
    {
        var navegationParameter = new ShellNavigationQueryParameters
        {
            {"Accommodation", _accommodation },
            {"index", 4 }
        };
        await Shell.Current.GoToAsync($"{nameof(EditAccommodationForm)}", navegationParameter);
    }

    [RelayCommand]
    private async Task EditAccommodationMediaAsync(Accommodation accommodation)
    {
        var navegationParameter = new ShellNavigationQueryParameters
        {
            {"Accommodation", _accommodation },
            {"index", 5 }
        };
        await Shell.Current.GoToAsync($"{nameof(EditAccommodationForm)}", navegationParameter);
    }

    [RelayCommand]
    private async Task EditAccommodationInformationAsync(Accommodation accommodation)
    {
        var navegationParameter = new ShellNavigationQueryParameters
        {
            {"Accommodation", _accommodation },
            {"index", 6 }
        };
        await Shell.Current.GoToAsync($"{nameof(EditAccommodationForm)}", navegationParameter);
    }

    [RelayCommand]
    private void DeleteAccommodation(Accommodation accommodation)
    {

    }

}
