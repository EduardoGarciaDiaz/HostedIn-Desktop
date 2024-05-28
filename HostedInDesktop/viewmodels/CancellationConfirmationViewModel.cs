using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using HostedInDesktop.Abstract;
using HostedInDesktop.Data.Models;
using HostedInDesktop.Data.Services;
using HostedInDesktop.Messages;
using HostedInDesktop.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostedInDesktop.viewmodels
{
    public partial class CancellationConfirmationViewModel : ObservableObject
    {
        MultimediaServiceImpl multimediaService = new MultimediaServiceImpl();

        [ObservableProperty]
        private string _title = "";

        [ObservableProperty]
        private ImageSource _imageSource;

        [ObservableProperty]
        private string _address = "";

        [ObservableProperty]
        private string _price = "";

        [ObservableProperty]
        private string _type = "";

        [ObservableProperty]
        private string _hostName = "";

        [ObservableProperty]
        private string _hostPhoneNumber = "";

        [ObservableProperty]
        private string _date = "";

        [ObservableProperty]
        private string _time = "";

        [ObservableProperty]
        private string _reason = "";

        [ObservableProperty]
        private Cancellation _cancellation;

        ISharedService _SharedService;

        public CancellationConfirmationViewModel (ISharedService sharedService)
        {
            _SharedService = sharedService;
            WeakReferenceMessenger.Default.Register<CancellationCreatedMessage>(this, (r, m) =>
            {
                Cancellation = m.Value;
                SetValues(Cancellation);
            });
            GetCancellation();
        }

        private void GetCancellation()
        {
            try
            {
                Cancellation = _SharedService.GetValue<Cancellation>("CreatedCancellation");
                SetValues(Cancellation);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void SetValues(Cancellation cancellation)
        {
            Title = cancellation.booking.accommodation.title;
            Address = cancellation.booking.accommodation.location.address;
            Price = $"${cancellation.booking.accommodation.nightPrice:F2} MXN por noche";
            HostName = cancellation.booking.accommodation.user.fullName;
            Type = cancellation.booking.accommodation.accommodationType;
            HostPhoneNumber = cancellation.booking.accommodation.user.phoneNumber;
            Date = cancellation.cancellationDate.ToShortDateString();
            Time = cancellation.cancellationDate.ToString("HH:mm:ss");
            Reason = cancellation.reason;
            _ = GetImage(cancellation.booking.accommodation._id);
        }

        [RelayCommand]
        public async Task GoBookings()
        {
            App.contentToShow = new BookingsView();
            await Shell.Current.GoToAsync(nameof(GuestView));
        }

        public async Task GetImage(string id)
        {
            try
            {
                byte[] image = await multimediaService.LoadMainImageAccommodation(id, 0);
                ImageSource = ImageSource.FromStream(() => new MemoryStream(image));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


    }
}
