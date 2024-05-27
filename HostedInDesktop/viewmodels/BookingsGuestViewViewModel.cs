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
using System.Windows.Input;

namespace HostedInDesktop.viewmodels;

public partial class BookingsGuestViewViewModel : ObservableObject
{
    public ObservableCollection<Booking> Bookings { get; } = new ObservableCollection<Booking>();
    readonly IBookingService _bookingService = new BookingService();

   
    [ObservableProperty]
    private bool isLoading;

    [ObservableProperty]
    private Color button1Color;

    [ObservableProperty]
    private Color button2Color;

    public BookingsGuestViewViewModel()
    {
        Button1Color = Colors.MediumPurple;
        Button2Color = Colors.WhiteSmoke;
    }
  


    [RelayCommand]
    private async Task LoadCurrentAccommodationsAsync()
    {
        if (IsLoading) return;
        try
        {
            IsLoading = true;
            var accommodations = await _bookingService.GetGuestBookings(App.user._id, BookingStatus.CURRENT.GetDescription());
            Bookings.Clear();
            foreach (var accommodation in accommodations)
            {
                Bookings.Add(accommodation);
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
    private async Task LoadOerduedAccommodationsAsync()
    {
        if (IsLoading) return;
        try
        {
            IsLoading = true;
            var accommodations = await _bookingService.GetGuestBookings(App.user._id, BookingStatus.OVERDUE.GetDescription());
            Bookings.Clear();
            foreach (var accommodation in accommodations)
            {
                Bookings.Add(accommodation);
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

    [RelayCommand]
    private void WatchBookingDetails(Booking booking)
    {
        Shell.Current.DisplayAlert("Si", "Veamos tu reservacion", "ok");
    }
}
