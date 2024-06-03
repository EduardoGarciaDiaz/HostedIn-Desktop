using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using HostedInDesktop.Abstract;
using HostedInDesktop.Data.Models;
using HostedInDesktop.Data.Services;
using HostedInDesktop.Enums;
using HostedInDesktop.Messages;
using HostedInDesktop.Utils;
using HostedInDesktop.Views;
using Syncfusion.Maui.Calendar;
using System.ComponentModel;


namespace HostedInDesktop.viewmodels
{
    public partial class AccommodationBookingViewModel : ObservableObject
    {
        private const string DEFAULT_PRICE_VALUE = "$ MXN";
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly IBookingService _bookingService = new BookingService();
        ISharedService _sharedService = new SharedService();

        [ObservableProperty]
        private Accommodation accommodationData;

        [ObservableProperty]
        private bool isLoading;

        [ObservableProperty]
        private int guestsNumber;

        [ObservableProperty]
        private string guestsNumberLimit;

        [ObservableProperty]    
        private string pricePerNights;

        [ObservableProperty]
        private string subtotal;

        [ObservableProperty]
        private string priceIVA;

        [ObservableProperty]
        private string totalPrice;

        [ObservableProperty]
        private ImageSource mainImage;

        [ObservableProperty]
        private string accommodationType;

        [ObservableProperty]
        private string nightPrice;

        [ObservableProperty]
        private ImageSource profilePhotoHost;

        [ObservableProperty]
        private CalendarDateRange selectedBookingDates;

        [ObservableProperty]
        private string startDateSelected;

        [ObservableProperty]
        private string endDateSelected;

        [ObservableProperty]
        private bool calendarBookingVisibility;

        private double finalPrice;

        public AccommodationBookingViewModel(ISharedService sharedService)
        {
            //get booked dates

            _sharedService = sharedService;
            WeakReferenceMessenger.Default.Register<AccommodationBookingMessage>(this, (r, m) =>
            {
                AccommodationData = m.Value;
                ResetPricesValues();
                LoadAccommodationData();
                CalendarBookingVisibility = false;
            });

            LoadAccommodationData();
            CalendarBookingVisibility = false;
        }

        [RelayCommand]
        public async void GoBack()
        {
            await Shell.Current.GoToAsync(nameof(AccommodationDetails));
        }

        [RelayCommand]
        public async void DecrementGuests()
        {
            if (GuestsNumber > 1)
            {
                GuestsNumber--;
            }
        }

        [RelayCommand]
        public async void IncrementGuests()
        {
            if (GuestsNumber < AccommodationData.guestsNumber)
            {
                GuestsNumber++;
            }
        }

        [RelayCommand]
        public async Task SaveBooking()
        {
            if (IsBookingValid())
            {
                Booking newBooking = CollectBookingData();
                if (newBooking != null)
                {
                    CreateBooking(newBooking);
                }
            }
        }

        [RelayCommand]
        public async void DateClicked()
        {
            if (!CalendarBookingVisibility)
            {
                CalendarBookingVisibility = true;
            } 
            else
            {
                CalendarBookingVisibility = false;
            }
        }

        public async void SelectBookingDates()
        {
            DateTime? startDate = SelectedBookingDates.StartDate;
            DateTime? endDate = SelectedBookingDates.EndDate;

            if (startDate != null && endDate != null)
            {
                TimeSpan dateDifference = (TimeSpan)(endDate - startDate);
                int daysBetween = dateDifference.Days;
                StartDateSelected = startDate.Value.ToShortDateString();
                EndDateSelected = endDate.Value.ToShortDateString();

                daysBetween++;

                LoadPricePerNights(daysBetween);
                double subtotal = LoadSubtotal(daysBetween);
                double priceIVA = LoadIVA(subtotal);
                LoadTotal(subtotal, priceIVA);
            }
        }

        private void LoadAccommodationData()
        {
            AccommodationData = _sharedService.GetValue<Accommodation>(AccommodationDetailsViewModel.ACCOMMODATION_KEY);

            LoadMainImage();
            LoadGuestsLimit();
            LoadAccommodationType();
            LoadNightPrice();
            LoadHostData();
        }

        private void LoadMainImage()
        {
            MainImage = "ic_house.png";

            if (AccommodationData.mainImage != null && AccommodationData.mainImage != null 
                && AccommodationData.mainImage.Length > 0)
            {
                byte[] imageData = AccommodationData.mainImage;
                MainImage = ImageSource.FromStream(() => new MemoryStream(imageData));
            }
        }

        private void LoadGuestsLimit()
        {
            GuestsNumber = 1;
            int maxGuests = AccommodationData.guestsNumber;
            string limit = $"Solo permite hasta {maxGuests} huéspedes";
            GuestsNumberLimit = limit;
        }

        private void LoadPricePerNights(int nights)
        {
            double pricePerNight = AccommodationData.nightPrice;
            string price = $"${pricePerNight} MXN por {nights} noches";
            PricePerNights = price;
        }

        private double LoadSubtotal(int nights)
        {
            double pricePerNight = AccommodationData.nightPrice;
            double subtotal = pricePerNight * nights;
            string price = $"${subtotal} MXN";
            Subtotal = price;

            return subtotal;
        }

        private double LoadIVA(double subtotal)
        {
            double ivaPercentage = .16;
            double priceIVA = subtotal * ivaPercentage;
            PriceIVA = $"${priceIVA} MXN";

            return priceIVA;
        }

        private void LoadTotal(double subtotal, double priceIVA)
        {
            double total = subtotal + priceIVA;
            finalPrice = total;
            TotalPrice = $"${total} MXN";
        }

        private void LoadAccommodationType()
        {
            string type = AccommodationData.accommodationType;

            if (!string.IsNullOrEmpty(type))
            {
                string accommodationTypeDescription = AccommodationTypeDescriptions.GetDescriptionForType(type);
                if (!string.IsNullOrEmpty(accommodationTypeDescription))
                {
                    AccommodationType = accommodationTypeDescription;
                }
            }
        }

        private void LoadNightPrice()
        {
            double nightPrice = AccommodationData.nightPrice;
            string price = $"${nightPrice} MXN por noche";
            NightPrice = price;
        }

        //public Func<DateTime, bool> SelectableDayPredicate => DateValidation;

        //private bool DateValidation(DateTime date)
        //{
        //    // Aquí puedes implementar la lógica para validar las fechas seleccionables
        //    // Por ejemplo, si tienes una lista de fechas bloqueadas, puedes permitir que solo se seleccionen las fechas que no están en esa lista
        //    // En este ejemplo, estoy permitiendo que se seleccionen todas las fechas futuras
        //    return date >= DateTime.Today;
        //}

        //protected void OnPropertyChanged(string propertyName)
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}

        private void LoadHostData()
        {
            ProfilePhotoHost = "ic_user.png";

            if (AccommodationData.user.profilePhoto != null && AccommodationData.user.profilePhoto.data != null && AccommodationData.user.profilePhoto.data.Length > 0)
            {
                byte[] imageData = AccommodationData.user.profilePhoto.data;
                ProfilePhotoHost = ImageSource.FromStream(() => new MemoryStream(imageData));
            }
        }

        private bool IsBookingValid()
        {
            bool isBookingValid = true;

            if (SelectedBookingDates == null || SelectedBookingDates.StartDate == null || SelectedBookingDates.EndDate == null)
            {
                Shell.Current.DisplayAlert("Fechas no válidas", "Debes seleccionar la fecha de inicio y la de fin", "Ok");
                isBookingValid = false;
            }

            return isBookingValid;
        }

        private Booking CollectBookingData()
        {
            Booking newBooking = new Booking();

            string beginDate = DateFormatterUtils.ParseDateForMongoDB(SelectedBookingDates.StartDate.ToString());
            string endDate = DateFormatterUtils.ParseDateForMongoDB(SelectedBookingDates.EndDate.ToString());

            newBooking.accommodation = AccommodationData;
            if (string.IsNullOrEmpty(beginDate) || string.IsNullOrEmpty(endDate))
            {
                Shell.Current.DisplayAlert("Error", "Las fechas de inicio y fin no pueden ser nulas", "Ok");
                return null;
            } 
            else
            {
                newBooking.beginningDate = beginDate;
                newBooking.endingDate = endDate;
            }
            
            newBooking.numberOfGuests = GuestsNumber;
            newBooking.bookingStatus = BookingStatus.CURRENT.GetDescription();
            newBooking.totalCost = finalPrice;
            newBooking.hostUser = AccommodationData.user;
            newBooking.guestUser = App.user;

            return newBooking;
        }

        private DateTime? FormatDate(string dateStr)
        {
            DateTime? dateParsed = null;

            if (!string.IsNullOrEmpty(dateStr))
            {
                try
                {
                    string[] dateParts = dateStr.Split(' ')[0].Split('/');
                    string formattedDate = $"{dateParts[0]}/{dateParts[1]}/{dateParts[2]}";

                    dateParsed = (DateTime)DateFormatterUtils.ParseStringToDate(dateStr);
                    return dateParsed;
                }
                catch (FormatException e)
                {
                    Shell.Current.DisplayAlert("Fecha no válida", "El formato de la fecha no es válido", "Ok");
                }
            } 
            else
            {
                Console.WriteLine("La fecha viene nula o vacía");
            }

            return dateParsed;
        }

        private async void CreateBooking(Booking bookingCreation)
        {
            if (IsLoading)
            {
                return;
            }

            if (Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
            {
                try
                {
                    IsLoading = true;

                    Booking newBooking = await _bookingService.CreateBooking(bookingCreation);

                    if (newBooking != null)
                    {
                        await Shell.Current.DisplayAlert("Reservación exitosa", "Reservación registrada con éxito", "Ok");

                        if (!App.hostMode)
                        {
                            App.contentToShow = new BookingsView();
                            await Shell.Current.GoToAsync(nameof(GuestView));
                        }
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
        }

        private void ResetPricesValues()
        {
            StartDateSelected = "";
            EndDateSelected = "";

            PricePerNights = DEFAULT_PRICE_VALUE;
            Subtotal = DEFAULT_PRICE_VALUE;
            PriceIVA = DEFAULT_PRICE_VALUE;
            TotalPrice = DEFAULT_PRICE_VALUE;
        }
    }
}
