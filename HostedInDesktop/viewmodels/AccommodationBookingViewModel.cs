using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HostedInDesktop.Abstract;
using HostedInDesktop.Data.Models;
using HostedInDesktop.Data.Services;
using HostedInDesktop.Enums;
using HostedInDesktop.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostedInDesktop.viewmodels
{
    public partial class AccommodationBookingViewModel : ObservableObject
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly IBookingService _bookingService = new BookingService();
        ISharedService _sharedService = new SharedService();

        [ObservableProperty]
        private Accommodation accommodationData;

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

        public AccommodationBookingViewModel(ISharedService sharedService)
        {
            //get booked dates

            _sharedService = sharedService;
            LoadAccommodationData();

        }

        [RelayCommand]
        public async void GoBack()
        {
            await Shell.Current.GoToAsync(nameof(GuestView));
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

        //TODO call this method when dates are selected
        private void LoadPricePerNights(int nights)
        {
            double pricePerNight = AccommodationData.nightPrice;
            string price = $"${pricePerNight} MXN por {nights} noches";
            PricePerNights = price;
        }

        private void LoadSubtotal(int nights)
        {
            double pricePerNight = AccommodationData.nightPrice;
            double subtotal = pricePerNight * nights;
            string price = $"${subtotal} MXN";
            Subtotal = price;
        }

        private void LoadIVA(int nights)
        {
            //TODO:
            //string price = $"${total} MXN";
            //PriceIVA = price;
        }

        private void LoadTotal(int nights)
        {
            //TotalPrice = $"${total} MXN";
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

    }
}
