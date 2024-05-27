﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HostedInDesktop.Data.Models;
using HostedInDesktop.Data.Services;
using HostedInDesktop.Utils;
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
        Shell.Current.DisplayAlert("Ir a editar", $"Vamos a editar tu accommodation: {accommodation.title}", "Ok");
    }

}
