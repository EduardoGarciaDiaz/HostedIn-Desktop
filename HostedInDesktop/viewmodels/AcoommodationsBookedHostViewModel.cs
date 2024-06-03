using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using HostedInDesktop.Abstract;
using HostedInDesktop.Data.Models;
using HostedInDesktop.Data.Services;
using HostedInDesktop.Helper;
using HostedInDesktop.Messages;
using HostedInDesktop.Utils;
using HostedInDesktop.Views;
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
        private readonly MultimediaServiceImpl _multimediaService = new MultimediaServiceImpl();

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
                    var accommodationViewModel = new AccommodationBookingsViewModel(accommodation, ServiceHelper.GetService<ISharedService>());
                    await accommodationViewModel.LoadBookingsAsync(_bookingService);
                    await LoadAccommodationImageAsync(accommodationViewModel).ConfigureAwait(false);
                    Accommodations.Add(accommodationViewModel); 
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

        public async Task LoadAccommodationImageAsync(AccommodationBookingsViewModel accommodation)
        {
            try
            {
                var imageBytes = await _multimediaService.LoadMainImageAccommodation(accommodation.Accommodation._id, 0);
                accommodation.Accommodation.mainImage = imageBytes;
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }


    }

    public partial class AccommodationBookingsViewModel : ObservableObject
    {
        [ObservableProperty]
        private Accommodation _accommodation;

       
        public ObservableCollection<Booking> Bookings { get; }


        ISharedService _sharedService;
        public AccommodationBookingsViewModel(Accommodation accommodation, ISharedService sharedService)
        {
            _sharedService = sharedService;
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
            catch (UnauthorizedAccessException)
            {
                await Shell.Current.DisplayAlert("La sesión caducó", "La sesión caducó debido a inactividad.", "Ir a inicio de sesión");
                return;
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
        }

        [RelayCommand]
        private async Task WatchBookingDetails(Booking booking)
        {
            try
            {
                if (booking is null)
                {
                    return;
                }
                MultimediaServiceImpl multimediaServiceImpl = new MultimediaServiceImpl();
                booking.accommodation.mainImage = await multimediaServiceImpl.LoadMainImageAccommodation(booking.accommodation._id, 0); 
                _sharedService.Add<Booking>("BookingDetail", booking);
                WeakReferenceMessenger.Default.Send(new BookingSelectedMessage(booking));
                await Shell.Current.GoToAsync(nameof(BookingDetailsView));
            }
            catch
            (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        

    }
}
