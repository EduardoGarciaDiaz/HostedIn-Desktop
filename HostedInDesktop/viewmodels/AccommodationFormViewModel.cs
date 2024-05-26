using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using HostedInDesktop.Views;
using Syncfusion.Maui.Core.Carousel;
using HostedInDesktop.viewmodels.ModelObservable;
using HostedInDesktop.Data.Models;
using HostedInDesktop.Utils;
using HostedInDesktop.Data.Services;

namespace HostedInDesktop.viewmodels
{
    public partial class AccommodationFormViewModel : ObservableObject
    {
        private readonly IAccommodationsService _accommodationService = new AccommodationsService();

        [ObservableProperty]
        private View _currentContentView;

        private int _currentViewIndex;
        public List<ContentView> ContentViews { get; }

        [ObservableProperty]
        private List<AccommodationType> accommodationTypes;

        [ObservableProperty]
        private AccommodationType selectedAccommodationType;

        private Location _selectedLocation;

        public Location SelectedLocation
        {
            get => _selectedLocation;
            set => SetProperty(ref _selectedLocation, value);
        }

        [ObservableProperty]
        private ObservableCollection<AccommodationBasic> accommodationBasics;

        [ObservableProperty]
        private List<AccommodationService> accommodationServices;

        [ObservableProperty]
        private string mainImage;

        [ObservableProperty]
        public string secondImage;

        [ObservableProperty]
        public string thirdImage;

        [ObservableProperty]
        public string videoPath;

        [ObservableProperty]
        private string accommodationTitle;
        
        [ObservableProperty]
        private string accommodationDescription;

        [ObservableProperty]
        private string accommodationRules;

        [ObservableProperty]
        private decimal accommodationNightPrice;

        public AccommodationFormViewModel()
        {
            _currentViewIndex = 0;

            ContentViews = new List<ContentView>
            {
                new AccommodationFormType() { BindingContext = this },
                new AccommodationFormLocation(this) { BindingContext = this },
                new AccommodationFormBasics() { BindingContext = this },
                new AccommodationFormServices() { BindingContext = this },
                new AccommodationFormMultimedia() { BindingContext = this },
                new AccommodationFormInformation() { BindingContext = this }
            };

            CurrentContentView = ContentViews[_currentViewIndex];
            AccommodationTypes = new List<AccommodationType>
            {
                new AccommodationType { Name = "Casa", Icon = "ic_house.png", BasicEnum = Enums.AccommodationTypes.house},
                new AccommodationType { Name = "Departamento", Icon = "ic_apartment.png" , BasicEnum = Enums.AccommodationTypes.apartment },
                new AccommodationType { Name = "Cabaña", Icon = "ic_cabin.png", BasicEnum = Enums.AccommodationTypes.cabin },
                new AccommodationType { Name = "Campamento", Icon = "ic_camp.png" , BasicEnum = Enums.AccommodationTypes.camp },
                new AccommodationType { Name = "Casa rodante", Icon = "ic_camper.png", BasicEnum = Enums.AccommodationTypes.camper },
                new AccommodationType { Name = "Barco", Icon = "ic_ship.png", BasicEnum = Enums.AccommodationTypes.ship },
            };

            AccommodationBasics = new ObservableCollection<AccommodationBasic>
            {
                new AccommodationBasic { BasicText = "Huéspedes", Value = 1 },
                new AccommodationBasic { BasicText = "Habitaciones", Value = 1 },
                new AccommodationBasic { BasicText = "Camas", Value = 1 },
                new AccommodationBasic { BasicText = "Baños", Value = 1 }
            };

            AccommodationServices = new List<AccommodationService>
            {
                new AccommodationService { ServiceName = "Internet", IsSelected = false, ServiceEnum = Enums.AccommodationServices.internet },
                new AccommodationService { ServiceName = "TV", IsSelected = false, ServiceEnum = Enums.AccommodationServices.tv },
                new AccommodationService { ServiceName = "Cocina", IsSelected = false, ServiceEnum = Enums.AccommodationServices.kitchen },
                new AccommodationService { ServiceName = "Lavadora", IsSelected = false, ServiceEnum = Enums.AccommodationServices.washing_machine },
                new AccommodationService { ServiceName = "Estacionamiento", IsSelected = false, ServiceEnum = Enums.AccommodationServices.parking },
                new AccommodationService { ServiceName = "Aire acondicionado", IsSelected = false, ServiceEnum = Enums.AccommodationServices.air_conditioning },
                new AccommodationService { ServiceName = "Alberca", IsSelected = false, ServiceEnum = Enums.AccommodationServices.pool },
                new AccommodationService { ServiceName = "Jardín", IsSelected = false, ServiceEnum = Enums.AccommodationServices.garden },
                new AccommodationService { ServiceName = "Luz", IsSelected = false, ServiceEnum = Enums.AccommodationServices.light },
                new AccommodationService { ServiceName = "Agua", IsSelected = false, ServiceEnum = Enums.AccommodationServices.water },
            };
        }

        [RelayCommand]
        public async Task Next()
        {
            if (_currentViewIndex < ContentViews.Count - 1)
            {
                _currentViewIndex++;
                CurrentContentView = ContentViews[_currentViewIndex];
            }
            else if (_currentViewIndex == ContentViews.Count - 1)
            {
                Accommodation newAccommodation = CreateAccommodation();
                PublishAccommodation(newAccommodation);
            }
        }

        [RelayCommand]
        public async Task Back()
        {
            if (_currentViewIndex > 0)
            {
                _currentViewIndex--;
                CurrentContentView = ContentViews[_currentViewIndex];
            } 
            else if (_currentViewIndex == 0)
            {
                await Shell.Current.GoToAsync(nameof(HostView));
            }
        }

        [RelayCommand]
        public void AccommodationTypeSelected(string type)
        {
            foreach (var accommodationType in AccommodationTypes)
            {
                accommodationType.IsSelected = false;
            }

            var selectedType = AccommodationTypes.FirstOrDefault(a => a.Name == type);

            if (selectedType != null)
            {
                selectedType.IsSelected = true;
                SelectedAccommodationType = selectedType;
            }
            else
            {
                Shell.Current.DisplayAlert("Ups", "Debes seleccionar el tipo de alojamiento", "OK");
                // Si no se encuentra un tipo de alojamiento seleccionado, puedes manejarlo aquí
            }
        }

        [RelayCommand]
        public void AccommodationServiceSelected(string service)
        {
            var selectedType = accommodationServices.FirstOrDefault(a => a.ServiceName == service);

            if (selectedType != null && !selectedType.IsSelected)
            {
                selectedType.IsSelected = true;
            }
            else if (selectedType != null && selectedType.IsSelected)
            {
                selectedType.IsSelected = false;
            }
        }

        [RelayCommand]
        public async Task SelectImage(string imageKey)
        {
            var result = await MediaPicker.PickPhotoAsync();

            if (result != null)
            {
                string imagePath = result.FullPath;

                switch (imageKey)
                {
                    case "MainImage":
                        MainImage = imagePath;
                        break;
                    case "SecondImage":
                        SecondImage = imagePath;
                        break;
                    case "ThirdImage":
                        ThirdImage = imagePath;
                        break;
                }
            }
        }

        [RelayCommand]
        public async Task SelectMedia(string mediaType)
        {
            FileResult result = null;

            if (mediaType == "Video")
            {
                result = await MediaPicker.PickVideoAsync();
            }

            if (result != null)
            {
                string mediaPath = result.FullPath;

                if (mediaType == "Video")
                {
                    VideoPath = mediaPath;
                }
            }
        }

        private Accommodation CreateAccommodation()
        {
            Accommodation accommodation = new Accommodation();
            accommodation.title = AccommodationTitle;
            accommodation.description = AccommodationDescription;
            accommodation.rules = AccommodationRules;

            accommodation.accommodationType = SelectedAccommodationType.BasicEnum.ToString();

            accommodation.nightPrice = (double)AccommodationNightPrice;
            accommodation.guestsNumber = AccommodationBasics[0].Value;
            accommodation.roomsNumber = AccommodationBasics[1].Value;
            accommodation.bedsNumber = AccommodationBasics[2].Value;
            accommodation.bathroomsNumber = AccommodationBasics[3].Value;            
            
            accommodation.accommodationServices = AccommodationServices.Where(a => a.IsSelected).Select(a => a.ServiceEnum.ToString().Replace("_", " ")).ToArray();

            accommodation.location = new Location
            {
                address = SelectedLocation.address,
                coordinates = new double[] { SelectedLocation.latitude, SelectedLocation.longitude },
                latitude = SelectedLocation.latitude,
                longitude = SelectedLocation.longitude,
                type = "Point"
            };

            accommodation.user = new User
            {
                _id = App.user._id
            };

            return accommodation;
        }

        public async void PublishAccommodation(Accommodation accommodationCreation)
        {
            if (Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
            {
                try
                {
                    if (IsAccommodationValid())
                    {
                        Accommodation newAccommodation = await _accommodationService.CreateAccommodationAsync(accommodationCreation);

                        if (newAccommodation != null)
                        {
                            await Shell.Current.DisplayAlert("Alojamiento creado", "Tu alojamiento se ha creado con éxito", "Ok");
                            ResetForm();
                            await Shell.Current.GoToAsync(nameof(HostView));
                        }
                    } 
                    else
                    {
                        await Shell.Current.DisplayAlert("Faltan datos", "Debes llenar el formulario completo", "Ok");
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
        }

        public bool IsAccommodationValid()
        {
            return true;
        }

        public void ResetForm()
        {
            _currentViewIndex = 0;
            CurrentContentView = ContentViews[_currentViewIndex];
            SelectedAccommodationType = null;
            SelectedLocation = null;

            AccommodationTypes.ForEach(a => a.IsSelected = false);

            AccommodationBasics = new ObservableCollection<AccommodationBasic>
            {
                new AccommodationBasic { BasicText = "Huéspedes", Value = 1 },
                new AccommodationBasic { BasicText = "Habitaciones", Value = 1 },
                new AccommodationBasic { BasicText = "Camas", Value = 1 },
                new AccommodationBasic { BasicText = "Baños", Value = 1 }
            };

            foreach (var service in AccommodationServices)
            {
                service.IsSelected = false;
            }

            MainImage = null;
            SecondImage = null;
            ThirdImage = null;
            VideoPath = null;

            AccommodationTitle = string.Empty;
            AccommodationDescription = string.Empty;
            AccommodationRules = string.Empty;
            AccommodationNightPrice = 0;
        }

    }



}
