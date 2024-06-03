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
using Google.Protobuf;
using CommunityToolkit.Maui.Views;
using NetTopologySuite.Operation.Valid;
using GoogleApi.Entities.Maps.AddressValidation.Response;

namespace HostedInDesktop.viewmodels
{
    public partial class AccommodationFormViewModel : ObservableObject
    {
        private const long MAX_SIZE_VIDEO_MB = 50 * 1024 * 1024; // 50 MB
        private const int MAX_SECONDS_DURATION_VIDEO = 300; // 5 minuteS
        private readonly IAccommodationsService _accommodationService = new AccommodationsService();
        private readonly MultimediaServiceImpl _multimediaService = new MultimediaServiceImpl();

        [ObservableProperty]
        private View _currentContentView;

        private int _currentViewIndex;
        public List<ContentView> ContentViews { get; }

        [ObservableProperty]
        private List<AccommodationType> accommodationTypes;

        [ObservableProperty]
        private bool isLoading;

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
                MakePublication();
            }
        }

        private async Task MakePublication()
        {
            if (IsAccommodationValid())
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
                App.hostMode = true;
                App.ContentViewHost = new HostAccommodationsView(new AccommodationsOwnedViewModel());
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
                    if (IsVideoValid(mediaPath))
                    {
                        VideoPath = mediaPath;
                    }
                }
            }
        }

        private Accommodation CreateAccommodation()
        {
            Accommodation accommodation = new Accommodation();
            accommodation.title = AccommodationTitle.Trim();
            accommodation.description = AccommodationDescription.Trim();
            accommodation.rules = AccommodationRules.Trim();

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
            if (IsLoading)
            {
                return;
            }

            if (Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
            {
                try
                {
                    IsLoading = true;


                    Accommodation newAccommodation = await _accommodationService.CreateAccommodationAsync(accommodationCreation);

                    if (newAccommodation != null)
                    {
                        await UploadMultimedias(newAccommodation._id);
                        await Shell.Current.DisplayAlert("Alojamiento creado", "Tu alojamiento se ha creado con éxito", "Ok");
                        ResetForm();
                        App.ContentViewHost = new HostAccommodationsView(new AccommodationsOwnedViewModel());
                        await Shell.Current.GoToAsync(nameof(HostView));
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

        public bool IsAccommodationValid()
        {
            bool isAccommodationValid = true;

            if (!IsTypeValid())
            {
                isAccommodationValid = false;
            }
            else if (!IsLocationValid())
            {
                isAccommodationValid = false;
            }
            else if (!IsBasicsValid())
            {
                isAccommodationValid = false;
            } 
            else if (!IsServicesValid())
            {
                isAccommodationValid = false;
            } 
            else if (!IsMultimediaValid())
            {
                isAccommodationValid = false;
            }
            else if (!IsInformationValid())
            {
                isAccommodationValid = false;
            }

            return isAccommodationValid;
        }

        private bool IsTypeValid()
        {
            bool isTypeValid = true;

            if (SelectedAccommodationType == null)
            {
                isTypeValid = false;
                Shell.Current.DisplayAlert("Tipo de alojamiento", "Debes seleccionar un tipo de alojamiento", "Ok");
            }

            return isTypeValid;
        }

        private bool IsLocationValid()
        {
            bool isLocationValid = true;

            if (SelectedLocation == null 
                || SelectedLocation.address == null 
                || SelectedLocation.latitude == 0 
                || SelectedLocation.longitude == 0)
            {
                isLocationValid = false;
                Shell.Current.DisplayAlert("Dirección", "Debes seleccionar una dirección válida", "Ok");
            }

            return isLocationValid;
        }

        private bool IsBasicsValid()
        {
            bool isBasicsValid = true;

            if (AccommodationBasics.Any(a => a.Value < 0))
            {
                isBasicsValid = false;
                Shell.Current.DisplayAlert("Datos básicos", "Debes seleccionar los datos básicos", "Ok");
            }

            return isBasicsValid;
        }

        private bool IsServicesValid()
        {
            bool isServicesValid = true;

            if (!AccommodationServices.Any(a => a.IsSelected))
            {
                isServicesValid = false;
                Shell.Current.DisplayAlert("Servicios", "Debes seleccionar al menos un servicio", "Ok");
            }

            return isServicesValid;
        }

        private bool IsMultimediaValid()
        {
            bool isMultimediaValid = true;

            if (string.IsNullOrEmpty(MainImage) || string.IsNullOrEmpty(SecondImage) || string.IsNullOrEmpty(ThirdImage) || string.IsNullOrEmpty(VideoPath))
            {
                isMultimediaValid = false;
                Shell.Current.DisplayAlert("Multimedia de tu alojamiento", "Debes seleccionar las 3 imagenes y 1 video", "Ok");
            }

            if (!string.IsNullOrEmpty(VideoPath) && !IsVideoValid(VideoPath))
            {
                isMultimediaValid = false;
            }

            return isMultimediaValid;
        }

        private bool IsVideoValid(string videoPath)
        {
            bool isValid = true;

            if (!IsVideoSizeValid(videoPath))
            {
                isValid = false;
            } 

            if (!IsVideoDurationValid(videoPath))
            {
                isValid = false;
            }           

            return isValid;
        }

        private bool IsVideoSizeValid(string videoPath)
        {
            bool isValid = true;

            FileInfo fileInfo = new FileInfo(videoPath);
            if (fileInfo.Length > MAX_SIZE_VIDEO_MB)
            {
                Shell.Current.DisplayAlert("Video demasiado grande", "El tamaño del video no debe exceder 50 MB.", "Ok");
                isValid = false;
            }

            return isValid;
        }

        private bool IsVideoDurationValid(string videoPath)
        {
            bool isValid = true;
            var inputFile = new MediaElement { Source = videoPath };

            var duration = inputFile.Duration;
            if (duration.TotalSeconds > MAX_SECONDS_DURATION_VIDEO)
            {
                Shell.Current.DisplayAlert("Video demasiado largo", "La duración del video no debe exceder los 5 minutos.", "Ok");
                isValid = false;
            }

            return isValid;
        }

        private bool IsInformationValid()
        {
            bool isInformationValid = true;

            if (!IsTitleValid())
            {
                isInformationValid = false;
            }
            else if (!IsDescriptionValid())
            {
                isInformationValid = false;
            } 
            else if (!IsRulesValid())
            {
                isInformationValid = false;
            }
            else if (!IsPriceValid())
            {
                isInformationValid = false;
            }

            return isInformationValid;
        }

        private bool IsTitleValid()
        {
            bool isTitleValid = true;

            if (string.IsNullOrEmpty(AccommodationTitle))
            {
                Shell.Current.DisplayAlert("Titulo obligatorio", "Ingresa un titulo para que tus futuros huespedes lo reconozcan", "Ok");
                isTitleValid = false;
            }
            else if (!DataValidator.IsAccommodationInformationValid(AccommodationTitle.Trim()))
            {
                Shell.Current.DisplayAlert("Titulo no válido", "Por favor, ingresa un titulo válido (al menos 5 caracteres)", "Ok");
                isTitleValid = false;
            }

                return isTitleValid;
        } 

        private bool IsDescriptionValid()
        {
            bool isDescriptionValid = true;

            if (string.IsNullOrEmpty(AccommodationDescription))
            {
                Shell.Current.DisplayAlert("Descripción obligatoria", "Describele a tus futuros huéspedes tu alojamiento", "Ok");
                isDescriptionValid = false;
            }
            else if (!DataValidator.IsAccommodationInformationValid(AccommodationDescription.Trim()))
            {
                Shell.Current.DisplayAlert("Descripción no válida", "Por favor, ingresa una descripción válido (de 5 a 500 caracteres)", "Ok");
                isDescriptionValid = false;
            }

            return isDescriptionValid;
        }

        private bool IsRulesValid()
        {
            bool isRulesValid = true;

            if (string.IsNullOrEmpty(AccommodationRules))
            {
                Shell.Current.DisplayAlert("Reglas obligatorias", "Especifica que está permitido y que no, para tener un orden en tu alojamiento", "Ok");
                isRulesValid = false;
            }
            else if (!DataValidator.IsAccommodationInformationValid(AccommodationRules.Trim()))
            {
                Shell.Current.DisplayAlert("Reglas no válidas", "Por favor, ingresa reglas válidas (de 5 a 500 caracteres)", "Ok");
                isRulesValid = false;
            }

            return isRulesValid;
        }

        private bool IsPriceValid()
        {
            bool isPriceValid = true;

            if (AccommodationNightPrice <= 0)
            {
                Shell.Current.DisplayAlert("Precio no válido", "Por favor, ingresa un precio válido", "Ok");
                isPriceValid = false;
            }

            return isPriceValid;
        }

        public void ResetForm()
        {
            _currentViewIndex = 0;
            CurrentContentView = ContentViews[_currentViewIndex];
            SelectedAccommodationType = null;

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

        private async Task UploadMultimedias(string accommodationId)
        {

            if (MainImage != null)
            {
                await UploadMultimedia(accommodationId, MainImage);
            }

            if (SecondImage != null)
            {
                await UploadMultimedia(accommodationId, SecondImage);
            }

            if (ThirdImage != null)
            {
                await UploadMultimedia(accommodationId, ThirdImage);
            }

            if (VideoPath != null)
            {
                await UploadMultimedia(accommodationId, VideoPath);
            }
        }

        private async Task UploadMultimedia(string accommodationId, string path)
        {
            ByteString[] bytesMultimedia = ImageHelper.ConvertPathToByteString(path);

            if (bytesMultimedia != null)
            {
                try
                {
                    var response = await _multimediaService.SaveImagesAccommodation(accommodationId, bytesMultimedia);
                    if (!response.Contains("Upload successful"))
                    {
                        await Shell.Current.DisplayAlert("Ups...", "No se pudo cargar un archivo multimedia, pero puedes modificarlo después", "Ok");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }



}
