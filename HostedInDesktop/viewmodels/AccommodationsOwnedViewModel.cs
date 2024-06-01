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
    private readonly MultimediaServiceImpl _multimediaService = new MultimediaServiceImpl();

    [ObservableProperty]
    private bool isLoading;

    [ObservableProperty]
    private ContentView _currentView;

    [ObservableProperty]
    private bool editVisitble;

    [ObservableProperty]
    private bool buttonVisitble;

    public AccommodationsOwnedViewModel()
    {
        LoadAccommodationsAsync();
        editVisitble = false;
        buttonVisitble = !editVisitble;
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
                LoadAccommodationImageAsync(accommodation);
            }
        }
        catch (UnauthorizedAccessException)
        {
            await Shell.Current.DisplayAlert("La sesión caducó", "La sesión caducó debido a inactividad.", "Ir a inicio de sesión");
            await Shell.Current.GoToAsync("///Login");
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

    private async Task LoadAccommodationImageAsync(Accommodation accommodation)
    {
        try
        {
            var imageBytes = await _multimediaService.LoadMainImageAccommodation(accommodation._id, 0);
            accommodation.mainImage = imageBytes;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    [RelayCommand]
    private void EditAccommodation(Accommodation accommodation)
    {
        CurrentView = new EditAccommodation(accommodation);
        EditVisitble = true;
        ButtonVisitble = !EditVisitble;
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
        EditVisitble = false;
        ButtonVisitble = !EditVisitble;
    }


}
