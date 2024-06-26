﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using HostedInDesktop.Abstract;
using HostedInDesktop.Data.Models;
using HostedInDesktop.Data.Services;
using HostedInDesktop.Messages;
using HostedInDesktop.Utils;
using HostedInDesktop.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostedInDesktop.viewmodels
{
    public partial class ExploreViewModel : ObservableObject
    {
        public ObservableCollection<Accommodation> Accommodations { get; } = new ObservableCollection<Accommodation>();
        public ObservableCollection<Place> Places { get; } = new ObservableCollection<Place>();

        private readonly MultimediaServiceImpl _multimediaService = new MultimediaServiceImpl();
        private readonly IAccommodationsService _accommodationsService = new AccommodationsService();
        private readonly IPlacesClient _placesClient = new PlacesClient();

        ISharedService _sharedService = new SharedService();

        [ObservableProperty]
        private bool isShowingPlaces;

        [ObservableProperty]
        private bool isLoading;

        [ObservableProperty]
        private string query;

        public ExploreViewModel(ISharedService sharedService)
        {
            LoadAccommodationsAsync();
            IsShowingPlaces = false;
            _sharedService = sharedService;
        }

        [RelayCommand]
        public async Task LoadAccommodationsAsync()
        {
            if (IsLoading) return;

            try
            {
                IsLoading = true;
                var accommodations = await _accommodationsService.GetAccommodationsAsync(App.user._id);
                Accommodations.Clear();
                foreach(var accommodation in accommodations)
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
            catch (ApiException aex)
                {
                await Shell.Current.DisplayAlert("Error", aex.Message, "Ok");
                return;
            }
            catch (Exception)
            {
                await Shell.Current.DisplayAlert("Error ", GenericExceptionMessage.GetDescription(ExceptionMessages.GENERIC_DESKTOP_EXCEPTION_MEESAGE), "Ok");
                return;
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private async Task OnAccommodationSelected(Accommodation selectedAccommodation)
        {
            if (selectedAccommodation != null)
            {
                _sharedService.Add<Accommodation>(AccommodationDetailsViewModel.ACCOMMODATION_KEY, selectedAccommodation);
                WeakReferenceMessenger.Default.Send(new AccommodationSelectedMessage(selectedAccommodation));
                await Shell.Current.GoToAsync(nameof(AccommodationDetails));
            }
        }

        [RelayCommand]
        public async Task OnSearchPressed()
        {
            try
            {
                IsLoading = true;
                IsShowingPlaces = false;
                if (!string.IsNullOrWhiteSpace(Query))
                {
                    var places = await _placesClient.GetPlaces(Query);
                    Places.Clear();
                    foreach (var place in places)
                    {
                        Places.Add(place);
                    }
                    IsShowingPlaces = true;
                }
                else
                {
                    await Shell.Current.DisplayAlert("Accion no permitida", "Ingresa un lugar para buscar", "Aceptar");
                }
            } catch (Exception)
            {
                await Shell.Current.DisplayAlert("Error", GenericExceptionMessage.GetDescription(ExceptionMessages.GENERIC_DESKTOP_EXCEPTION_MEESAGE), "Aceptar");
            }
            finally {
                IsLoading = false;
            }
        }

        [RelayCommand]
        public async Task OnReloadPressed()
        {
            LoadAccommodationsAsync();
        }

        public async Task OnPlaceSelected(Place selectedPlace)
        {
            IsShowingPlaces = false;
            if (IsLoading) return;

            try
            {
                IsLoading = true;
                var accommodations = await _accommodationsService.GetAccommodationsAsync(App.user._id, 
                    selectedPlace.Geometry.Location.Latitude, selectedPlace.Geometry.Location.Longitude);
                Accommodations.Clear();
                foreach (var accommodation in accommodations)
                {
                    Accommodations.Add(accommodation);
                    _ = LoadAccommodationImageAsync(accommodation);
                }
            }
            catch (UnauthorizedAccessException)
            {
                await Shell.Current.DisplayAlert("La sesión caducó", "La sesión caducó debido a inactividad.", "Ir a inicio de sesión");
                await Shell.Current.GoToAsync("///Login");
            }
            catch (ApiException aex)
            {
                await Shell.Current.DisplayAlert("Error", aex.Message, "Ok");
                return;
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error ", GenericExceptionMessage.GetDescription(ExceptionMessages.GENERIC_DESKTOP_EXCEPTION_MEESAGE), "Ok");
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

    }
}
