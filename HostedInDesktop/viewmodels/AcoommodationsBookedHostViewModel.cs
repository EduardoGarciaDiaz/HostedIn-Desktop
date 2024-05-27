using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HostedInDesktop.Data.Models;
using HostedInDesktop.Data.Services;
using HostedInDesktop.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace HostedInDesktop.viewmodels
{
    public partial class AcoommodationsBookedHostViewModel : ObservableObject
    {
        public ObservableCollection<AccommodationBookingsViewModel> Accommodations { get; } = new ObservableCollection<AccommodationBookingsViewModel>();
        private readonly IAccommodationsService _accommodationsService = new AccommodationsService();
        private readonly IBookingService _bookingService = new BookingService();

        [ObservableProperty]
        private bool isLoading;

        public AcoommodationsBookedHostViewModel()
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
                var accommodations = await _accommodationsService.GetHostBookedAccommodationAsync(App.user._id);
                Accommodations.Clear();
                foreach (var accommodation in accommodations)
                {
                    var accommodationViewModel = new AccommodationBookingsViewModel(accommodation);
                    await accommodationViewModel.LoadBookingsAsync(_bookingService);
                    Accommodations.Add(accommodationViewModel);
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
    }

    public partial class AccommodationBookingsViewModel : ObservableObject
    {
        private readonly Accommodation _accommodation;

        public string AccommodationTitle => _accommodation.title;
        public string AccommodationDescription => _accommodation.description;
        public string AccommodationPrize => _accommodation.nightPrice.ToString();
        //public ImageSource AccommodationImage => ImageSource.FromStream(() => new MemoryStream(_accommodation.mainImage));
        public ObservableCollection<Booking> Bookings { get; }


        public AccommodationBookingsViewModel(Accommodation accommodation)
        {
            _accommodation = accommodation;
            Bookings = new ObservableCollection<Booking>();
        }


        public async Task LoadBookingsAsync(IBookingService bookingService)
        {
            try
            {
                var bookings = await bookingService.GetBookingsByAccommodationId(_accommodation._id);
                Bookings.Clear();
                foreach (var booking in bookings)
                {
                    Bookings.Add(booking);
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
        }

        [RelayCommand]
        private void WatchBookingDetails(Booking booking)
        {
            Shell.Current.DisplayAlert("Ir a detalles", $"Ver detalles reservacion: {booking.guestUser.fullName}", "Ok");
        }

    }
}
