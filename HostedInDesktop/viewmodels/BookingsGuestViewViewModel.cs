using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HostedInDesktop.Abstract;
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
using System.Windows.Input;

namespace HostedInDesktop.viewmodels;

public partial class BookingsGuestViewViewModel : ObservableObject
{
    public ObservableCollection<Booking> Bookings { get; } = new ObservableCollection<Booking>();
    readonly IBookingService _bookingService = new BookingService();
    private readonly MultimediaServiceImpl _multimediaService = new MultimediaServiceImpl();
    public ICommand ItemSelectedCommand { get; }

    [ObservableProperty]
    private bool isLoading;

    [ObservableProperty]
    private Color button1Color;

    [ObservableProperty]
    private Color button2Color;

    ISharedService _sharedService;

    public BookingsGuestViewViewModel(ISharedService sharedService)
    {
        ItemSelectedCommand = new RelayCommand<Booking>(OnItemSelected);
        LoadCurrentBookingsAsync();
        _sharedService = sharedService;
    }

    private async void OnItemSelected(Booking bookingSelected) {
        try
        {
            if (bookingSelected is null)
            {
                return;
            }
            _sharedService.Add<Booking>("BookingDetail", bookingSelected);
            await Shell.Current.GoToAsync(nameof(BookingDetailsView));
        }
        catch
        (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    [RelayCommand]
    private async Task LoadCurrentBookingsAsync()
    {
        if (IsLoading) return;
        try
        {
            IsLoading = true;
            var bookings = await _bookingService.GetGuestBookings(App.user._id, BookingStatus.CURRENT.GetDescription());
            Bookings.Clear();
            foreach (var booking in bookings)
            {
                Bookings.Add(booking);
                LoadAccommodationImageAsync(booking.accommodation);
            }
            Button1Color = Colors.MediumPurple;
            Button2Color = Colors.WhiteSmoke;
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
    private async Task LoadOverduedBookingsAsync()
    {
        if (IsLoading) return;
        try
        {
            IsLoading = true;
            var bookings = await _bookingService.GetGuestBookings(App.user._id, BookingStatus.OVERDUE.GetDescription());
            Bookings.Clear();
            foreach (var booking in bookings)
            {
                Bookings.Add(booking);
                LoadAccommodationImageAsync(booking.accommodation);
            }
            Button2Color = Colors.MediumPurple;
            Button1Color = Colors.WhiteSmoke;
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

    /* private void WatchBookingDetails(Booking booking)
     {
         try
         {
             if (booking is null)
             {
                 return;
             }
             _SharedService.Add<Booking>("BookingDetail", booking);
             Shell.Current.GoToAsync(nameof(BookingDetailsView));
         }
         catch (Exception ex)
         {
             Console.WriteLine(ex.ToString());
         }
     }*/
}
