using CommunityToolkit.Mvvm.ComponentModel;
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
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostedInDesktop.viewmodels
{
    public partial class BookingDetailsViewModel : ObservableObject
    {
        MultimediaServiceImpl multimediaService = new MultimediaServiceImpl();

        [ObservableProperty]
        private Booking _selectedBooking;

        [ObservableProperty]
        private byte[] _image;

        [ObservableProperty]
        private string _startDate;

        [ObservableProperty]
        private string _endDate;

        [ObservableProperty]
        private string _title;

        [ObservableProperty]
        private int _personsNumber;

        [ObservableProperty]
        private string _totalCost;

        [ObservableProperty]
        private string person;

        [ObservableProperty]
        private ImageSource _imageSource;

        [ObservableProperty]
        private String _selectetedBookingStatus;

        [ObservableProperty]
        private bool isRatingButtonVisible;

        [ObservableProperty]
        private bool _isCancelButtonVisible = true;


        readonly ISharedService _SharedService;

        public BookingDetailsViewModel(ISharedService sharedService)
        {
            try
            {
                _SharedService = sharedService;
                WeakReferenceMessenger.Default.Register<BookingSelectedMessage>(this, (r, m) =>
                {
                    SelectedBooking = m.Value;
                    SetValues(SelectedBooking);
                });
                GetSelectedBooking();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void GetSelectedBooking()
        {
            try
            {
                SelectedBooking = _SharedService.GetValue<Booking>("BookingDetail");
                SetValues(SelectedBooking);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private async Task SetValues(Booking selectedBooking)
        {
            Image = selectedBooking.accommodation.mainImage;
            StartDate = DateFormatterUtils.ConvertToReadableDate(selectedBooking.beginningDate);
            EndDate = DateFormatterUtils.ConvertToReadableDate(selectedBooking.endingDate);
            Title = selectedBooking.accommodation.title;
            PersonsNumber = selectedBooking.numberOfGuests;
            TotalCost = $"${selectedBooking.totalCost:F2} MXN";
            SelectetedBookingStatus = TranslatorToSpanish.TranslateBookingStatusValue(selectedBooking.bookingStatus);
            if (App.hostMode
                || selectedBooking.bookingStatus.Equals(BookingStatus.CURRENT.GetDescription())
                || selectedBooking.bookingStatus == BookingStatus.CANCELLED.GetDescription())
            {
                IsRatingButtonVisible = false;
            }
            else
            {
                IsRatingButtonVisible = true;
            }
            if (selectedBooking.bookingStatus == BookingStatus.CANCELLED.GetDescription()
                || !MoreThanTwentyFourHours(selectedBooking.beginningDate))
            {
                IsCancelButtonVisible = false;
            }
            else
            {
                IsCancelButtonVisible = true;
            }
            _ = GetImage(selectedBooking.accommodation._id);
            if (!App.hostMode)
            {
                Person = selectedBooking.accommodation.user.fullName;
            }
            else
            {
                Person = selectedBooking.guestUser.fullName;
            }
        }

        [RelayCommand]
        public async Task OnCancelClicked()
        {
            if (SelectedBooking is null)
            {
                return;
            }
            _SharedService.Add<Booking>("BookingToCancel", SelectedBooking);
            WeakReferenceMessenger.Default.Send(new BookingToCancelMessage(SelectedBooking));
            await Shell.Current.GoToAsync(nameof(CancelationReasonsView));
        }

        [RelayCommand]
        public async Task OnRateBookingClicked()
        {
            var navegationParameters = new ShellNavigationQueryParameters
            {
                {"Booking", SelectedBooking}
            };
            await Shell.Current.GoToAsync(nameof(AccommodationBookingReview), navegationParameters);
        }

        [RelayCommand]
        public async Task GoBack()
        {
            if (App.hostMode)
            {
                App.ContentViewHost = new HostBookedAccommodations(new AcoommodationsBookedHostViewModel());
                await Shell.Current.GoToAsync(nameof(HostView));
            }
            else
            {
                App.contentToShow = new BookingsView();
                await Shell.Current.GoToAsync(nameof(GuestView));
            }
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


        public string ConvertToReadableDate(string mongoDate)
        {
            if (DateTime.TryParse(mongoDate, out DateTime date))
            {
                return date.ToString("dd 'de' MMMM 'de' yyyy", new CultureInfo("es-ES"));
            }
            return string.Empty;
        }

        private bool MoreThanTwentyFourHours(string dateString)
        {
            if (DateTime.TryParse(dateString, null, System.Globalization.DateTimeStyles.RoundtripKind, out DateTime date))
            {
                DateTime now = DateTime.Now;
                TimeSpan difference = date - now;
                return difference.TotalHours > 24;
            }
            return false;
        }
    }
}
