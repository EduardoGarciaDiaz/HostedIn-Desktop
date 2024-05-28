using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HostedInDesktop.Data.Models;
using HostedInDesktop.Data.Services;
using HostedInDesktop.Utils;
using HostedInDesktop.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostedInDesktop.viewmodels;

public partial class AccommodationsOwnedViewModel : ObservableObject
{
    public ObservableCollection<Accommodation> Accommodations { get; } = new ObservableCollection<Accommodation>();
    readonly IAccommodationsService _accommodationsService = new AccommodationsService();

    [ObservableProperty]
    private bool isLoading;

    [ObservableProperty]
    private ContentView _currentView;

    public AccommodationsOwnedViewModel()
    {
        LoadAccommodationsAsync();
    }

    [RelayCommand]
    private async Task LoadAccommodationsAsync()
    {
        if (IsLoading) return;
        try
        {
            IsLoading = true;
            var accommodations = await _accommodationsService.GetHostOwnedAccommodationsAsync(App.user._id);
            Accommodations.Clear();
            foreach (var accommodation in accommodations)
            {
                Accommodations.Add(accommodation);
            }
        }
        catch (ApiException ex)
        {
            await Shell.Current.DisplayAlert("Error", ex.Message, "Ok");
            return;
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error ", ex.Message, "Ok");
            return;
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private void EditAccommodation(Accommodation accommodation)
    {
        CurrentView = new EditAccommodation(new EditAccommodationViewModel(accommodation));
    }

    [RelayCommand]
    private void CreateAccommodation()
    {
        Shell.Current.GoToAsync(nameof(AccommodationForm));
    }


    [RelayCommand]
    private void CloseEditAccommodation()
    {
        CurrentView = new ContentView();
    }


}
