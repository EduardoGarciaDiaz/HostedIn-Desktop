using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HostedInDesktop.Data.Models;
using HostedInDesktop.Data.Services;
using HostedInDesktop.Utils;
using HostedInDesktop.viewmodels.ModelObservable;
using HostedInDesktop.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using static Microsoft.Maui.ApplicationModel.Permissions;

namespace HostedInDesktop.viewmodels;

public partial class EditAccommodationFormViewModel : ObservableObject, INotifyPropertyChanged, IQueryAttributable
{
    private readonly IAccommodationsService _accommodationService = new AccommodationsService();

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        Accommodation = query["Accommodation"] as Accommodation;
        String index = HttpUtility.UrlDecode(query["index"].ToString());
        accommodationToEdit = Accommodation;
        OnPropertyChanged(nameof(Accommodation));
        OnPropertyChanged(nameof(accommodationToEdit));
        _currentViewIndex = int.Parse(index);
        ChooseView();
    }

  
    [ObservableProperty]
    private Accommodation accommodationToEdit;

    public Accommodation Accommodation { get; private set; }


    [ObservableProperty]
    private View _currentContentView;

    public int _currentViewIndex { get; private set; }

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

    public EditAccommodationFormViewModel()
    {
    }

    private void ChooseView()
    {
        switch (_currentViewIndex)
        {
            case 1:
                CurrentContentView = new AccommodationFormType();
                AccommodationTypes = new List<AccommodationType>
                {
                    new AccommodationType { Name = "Casa", Icon = "ic_house.png", BasicEnum = Enums.AccommodationTypes.house},
                    new AccommodationType { Name = "Departamento", Icon = "ic_apartment.png" , BasicEnum = Enums.AccommodationTypes.apartment },
                    new AccommodationType { Name = "Cabaña", Icon = "ic_cabin.png", BasicEnum = Enums.AccommodationTypes.cabin },
                    new AccommodationType { Name = "Campamento", Icon = "ic_camp.png" , BasicEnum = Enums.AccommodationTypes.camp },
                    new AccommodationType { Name = "Casa rodante", Icon = "ic_camper.png", BasicEnum = Enums.AccommodationTypes.camper },
                    new AccommodationType { Name = "Barco", Icon = "ic_ship.png", BasicEnum = Enums.AccommodationTypes.ship },
                };
                AccommodationTypeSelected(TranslateType(AccommodationToEdit.accommodationType));
                break;
            case 2:
                CurrentContentView = new AccommodationFormLocation(this);
                break;
            case 3:
                CurrentContentView = new AccommodationFormBasics();

                AccommodationBasics = new ObservableCollection<AccommodationBasic>
                {
                    new AccommodationBasic { BasicText = "Huéspedes", Value = accommodationToEdit.guestsNumber },
                    new AccommodationBasic { BasicText = "Habitaciones", Value = accommodationToEdit.roomsNumber },
                    new AccommodationBasic { BasicText = "Camas", Value = accommodationToEdit.bedsNumber },
                    new AccommodationBasic { BasicText = "Baños", Value = accommodationToEdit.bathroomsNumber }
                };
                break;
            case 4:
                CurrentContentView = new AccommodationFormServices();
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
                foreach (var item in accommodationToEdit.accommodationServices)
                {
                    AccommodationServiceSelected(TranslateService(item));
                }
                break;
            case 5:
                CurrentContentView = new AccommodationFormMultimedia();

                break;
            case 6:
                CurrentContentView = new AccommodationFormInformation();
                AccommodationTitle = AccommodationToEdit.title;
                AccommodationDescription = AccommodationToEdit.description;
                AccommodationRules = AccommodationToEdit.rules;
                AccommodationNightPrice = decimal.Parse(AccommodationToEdit.nightPrice.ToString());
                break;
        }
    }
   

    private string TranslateType(string type)
    {
        string spanish = "";
        if (type.Equals("house"))
        {
            spanish = "Casa";
        }else if (type.Equals("apartment"))
        {
            spanish = "Departamento";
        }
        else if (type.Equals("cabin"))
        {
            spanish = "Cabaña";
        }
        else if (type.Equals("camp"))
        {
            spanish = "Campamento";
        }
        else if (type.Equals("camper"))
        {
            spanish = "Casa rodante";
        }
        else if (type.Equals("ship"))
        {
            spanish = "Barco";
        }
        return spanish;
    }


    private string TranslateService(string service)
    {
        string spanish = "";
        if (service.Equals("internet"))
        {
            spanish = "Internet";
        }
        else if (service.Equals("tv"))
        {
            spanish = "TV";
        }
        else if (service.Equals("kitchen"))
        {
            spanish = "Cocina";
        }
        else if (service.Equals("washing_machine"))
        {
            spanish = "Lavadora";
        }
        else if (service.Equals("parking"))
        {
            spanish = "Estacionamiento";
        }
        else if (service.Equals("air_conditioning"))
        {
            spanish = "Aire acondicionado";
        }
        else if (service.Equals("pool"))
        {
            spanish = "Alberca";
        }
        else if (service.Equals("garden"))
        {
            spanish = "Jardín";
        }
        else if (service.Equals("light"))
        {
            spanish = "Luz";
        }
        else if (service.Equals("water"))
        {
            spanish = "Agua";
        }
        return spanish;
    }

    [RelayCommand]
    public async Task SaveChanges()
    {
        UpdateAccommodationObject();
        if (Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
        {
            try
            {
                if (IsAccommodationValid())
                {
                    Accommodation newAccommodation = await _accommodationService.UpdateAccommodation(accommodationToEdit);
                    if (newAccommodation != null)
                    {
                        await Shell.Current.DisplayAlert("Alojamiento actualizado", "Sehan guardado los cambios de tu alojamiento con éxito", "Ok");                      
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

    [RelayCommand]
    public void AccommodationTypeSelected(string type)
    {
        foreach (var accommodationType in AccommodationTypes)
        {
            accommodationType.IsSelected = false;
        }

        var selectedType = AccommodationTypes.FirstOrDefault(a => a.Name == type);
        var selectetTypeEn = AccommodationTypes.ToString().FirstOrDefault(a => a.Equals(type));
        if (selectedType != null)
        {
            
            selectedType.IsSelected = true;
            SelectedAccommodationType = selectedType;
        }
        else if (selectetTypeEn != null)
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



    private void UpdateAccommodationObject()
    {

        switch (_currentViewIndex)
        {
            case 1:

                accommodationToEdit.accommodationType = SelectedAccommodationType.BasicEnum.ToString();
                break;
            case 2:
                accommodationToEdit.location = new Location
                {
                    address = SelectedLocation.address,
                    coordinates = new double[] { SelectedLocation.latitude, SelectedLocation.longitude },
                    latitude = SelectedLocation.latitude,
                    longitude = SelectedLocation.longitude,
                    type = "Point"
                };
                break;
            case 3:
                accommodationToEdit.guestsNumber = AccommodationBasics[0].Value;
                accommodationToEdit.roomsNumber = AccommodationBasics[1].Value;
                accommodationToEdit.bedsNumber = AccommodationBasics[2].Value;
                accommodationToEdit.bathroomsNumber = AccommodationBasics[3].Value;
                break;
            case 4:
                accommodationToEdit.accommodationServices = AccommodationServices.Where(a => a.IsSelected).Select(a => a.ServiceEnum.ToString().Replace("_", " ")).ToArray();
                break;
            case 5:
                CurrentContentView = new AccommodationFormMultimedia();
                break;
            case 6:
                accommodationToEdit.title = AccommodationTitle;
                accommodationToEdit.description = AccommodationDescription;
                accommodationToEdit.rules = AccommodationRules;
                accommodationToEdit.nightPrice = (double)AccommodationNightPrice;
                break;
        } 
        
    }
    public bool IsAccommodationValid()
    {
        return true;
    }

    
}
