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
        private readonly IAccommodationsService _accommodationsService = new AccommodationsService();


        [ObservableProperty]
        private bool isLoading;

        public ExploreViewModel()
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
                var accommodations = await _accommodationsService.GetAccommodationsAsync(App.user._id);
                Accommodations.Clear();
                foreach(var accommodation in accommodations)
                {
                    Accommodations.Add(accommodation);
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

    }
}
