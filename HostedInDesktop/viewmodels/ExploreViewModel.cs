using CommunityToolkit.Mvvm.ComponentModel;
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

namespace HostedInDesktop.viewmodels
{
    public partial class ExploreViewModel : ObservableObject
    {
        public ObservableCollection<Accommodation> Accommodations { get; } = new ObservableCollection<Accommodation>();
        public ObservableCollection<Place> Places { get; } = new ObservableCollection<Place>();

        private readonly MultimediaServiceImpl _multimediaService = new MultimediaServiceImpl();
        private readonly IAccommodationsService _accommodationsService = new AccommodationsService();
        private readonly IPlacesClient _placesClient = new PlacesClient();

        [ObservableProperty]
        private bool isShowingPlaces;

        [ObservableProperty]
        private bool isLoading;

        [ObservableProperty]
        private string query;

        public ExploreViewModel()
        {
            LoadAccommodationsAsync();
            IsShowingPlaces = false;
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
            catch(ApiException aex)
                {
                await Shell.Current.DisplayAlert("Error", aex.Message, "Ok");
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
        private async Task OnAccommodationSelected(Accommodation selectedAccommodation)
        {
            // Maneja el evento de selección aquí
            // Por ejemplo, navega a otra página o muestra detalles del alojamiento
            if (selectedAccommodation != null)
            {
                await Shell.Current.DisplayAlert("Ejemplo ", selectedAccommodation.title, "Ok");
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
            } catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "Aceptar");
            }
            finally {
                IsLoading = false;
            }
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
            catch (ApiException aex)
            {
                await Shell.Current.DisplayAlert("Error", aex.Message, "Ok");
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

    }
}
