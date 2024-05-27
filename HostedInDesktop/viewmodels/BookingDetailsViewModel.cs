using CommunityToolkit.Mvvm.ComponentModel;
using HostedInDesktop.Abstract;
using HostedInDesktop.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostedInDesktop.viewmodels
{
    public partial class BookingDetailsViewModel : ObservableObject
    {
        [ObservableProperty]
        private Booking selectedBooking;

        readonly ISharedService _SharedService;

        public BookingDetailsViewModel(ISharedService sharedService)
        {
            try
            {
                _SharedService = sharedService;
                GetSelectedBooking();
            } catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void GetSelectedBooking()
        {
            try
            {
                SelectedBooking = _SharedService.GetValue<Booking>("BookingDetail");
                Shell.Current.DisplayAlert("Booking", SelectedBooking.accommodation.title, "ok");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
