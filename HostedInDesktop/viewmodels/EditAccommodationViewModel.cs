

using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HostedInDesktop.Data.Models;
using HostedInDesktop.Data.Services;
using HostedInDesktop.Utils;
using HostedInDesktop.viewmodels.ModelObservable;
using HostedInDesktop.Views;
using Microsoft.Maui.Controls;
using Newtonsoft.Json;
using SkiaSharp.Views.Maui.Controls;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace HostedInDesktop.viewmodels;

public partial class EditAccommodationViewModel : ObservableObject
{

    public ObservableCollection<ImageSource> MultimediaItems { get; } = new ObservableCollection<ImageSource>();
    private IAccommodationsService _accommodationsService;

    [ObservableProperty]
    private String videoFilePath;

    [ObservableProperty]
    private ImageSource imageSource;
     
    private readonly MultimediaServiceImpl _multimediaService = new MultimediaServiceImpl();


    [ObservableProperty]
    private Accommodation accommodation;

    [ObservableProperty]
    private bool isImage;
    [ObservableProperty]
    private bool isVideo;
    [ObservableProperty]
    private int index;
    [ObservableProperty]
    private bool areImagesLoaded = false;


    private ImageSource imageSource1;
    private ImageSource imageSource2;
    private ImageSource imageSource3;


    public EditAccommodationViewModel()
    {
        isImage = true;
        isVideo = false;
        index = 0;
    }

    partial void OnAccommodationChanged(Accommodation value) { LoadImagesAsync();}



    private async void LoadImagesAsync()
    {
        try
        {
            var imageBytes1 = await _multimediaService.LoadMainImageAccommodation(Accommodation._id, 0);
            var imageBytes2 = await _multimediaService.LoadMainImageAccommodation(Accommodation._id, 1);
            var imageBytes3 = await _multimediaService.LoadMainImageAccommodation(Accommodation._id, 2);
            var videoBytes4 = await _multimediaService.LoadMainImageAccommodation(Accommodation._id, 3);            
            if (imageBytes1 == null)
            {
                imageSource1 = ImageSource.FromFile("img_provisional.png");
                imageSource2 = ImageSource.FromFile("img_provisional.png");
                imageSource3 = ImageSource.FromFile("img_provisional.png");
                VideoFilePath = "";
            }
            else
            {
                imageSource1 = ImageSource.FromStream(() => new MemoryStream(imageBytes1));
                imageSource2 = ImageSource.FromStream(() => new MemoryStream(imageBytes2));
                imageSource3 = ImageSource.FromStream(() => new MemoryStream(imageBytes3));
                VideoFilePath = await ImageHelper.SaveVideoToFileAsync(videoBytes4);
            }                    
        }
        catch (Exception e)
        {
            imageSource1 = ImageSource.FromFile("img_provisional.png");
            imageSource2 = ImageSource.FromFile("img_provisional.png");
            imageSource3 = ImageSource.FromFile("img_provisional.png");
            VideoFilePath = "";
            Console.WriteLine(e.Message);
        }
        finally
        {
            MultimediaItems.Clear();
            MultimediaItems.Add(imageSource1);
            MultimediaItems.Add(imageSource2);
            MultimediaItems.Add(imageSource3);
            ImageSource = MultimediaItems[0];
            AreImagesLoaded = true;
        }
    }


    [RelayCommand]
    private void GoBack()
    {
        Index--;
        if (Index == -1)
        {
            IsVideo = true;
            IsImage = false;
            Index = 3;

            return;
        }
        else if (Index == 2)
        {
            IsVideo = false;
            IsImage = true;
        }
        ImageSource = MultimediaItems[Index];        
    }

    [RelayCommand] 
    private void GoAhead()
    {
        Index++;
        if (Index == 3)
        {
            IsVideo = true;
            IsImage = false;
            return;
        }
        else if (Index == 4)
        {
            IsVideo = false;
            IsImage = true;
            Index = 0;

        }
        ImageSource = MultimediaItems[Index];
    }

    [RelayCommand]
    private async Task EditAccommodationTypeAsync()
    {
        var navegationParameter = new ShellNavigationQueryParameters
        {
            {"Accommodation", Accommodation },
            {"index", 1 }
        };
        await Shell.Current.GoToAsync($"{nameof(EditAccommodationForm)}", navegationParameter);
    }

    [RelayCommand]
    private async Task EditAccommodationUbicationAsync()
    {
        var navegationParameter = new ShellNavigationQueryParameters
        {
            {"Accommodation", Accommodation },
            {"index", 2 }
        };
        await Shell.Current.GoToAsync($"{nameof(EditAccommodationForm)}", navegationParameter);
    }

    [RelayCommand]
    private async Task EditAccommodationRoomsNumbersAsync()
    {
        var navegationParameter = new ShellNavigationQueryParameters
        {
            {"Accommodation", Accommodation },
            {"index", 3 }
        };
        await Shell.Current.GoToAsync($"{nameof(EditAccommodationForm)}", navegationParameter);
    }

    [RelayCommand]
    private async Task EditAccommodationServicesAsync()
    {
        var navegationParameter = new ShellNavigationQueryParameters
        {
            {"Accommodation", Accommodation },
            {"index", 4 }
        };
        await Shell.Current.GoToAsync($"{nameof(EditAccommodationForm)}", navegationParameter);
    }

    [RelayCommand]
    private async Task EditAccommodationMediaAsync()
    {
        if (AreImagesLoaded)
        {

            var navegationParameter = new ShellNavigationQueryParameters
            {
                {"Accommodation", Accommodation },
                {"index", 5 },
                {"MainImage", MultimediaItems[0] },
                {"SecondImage", MultimediaItems[1] },
                {"ThirdImage", MultimediaItems[2] },
                {"video", VideoFilePath }
            };
            await Shell.Current.GoToAsync($"{nameof(EditAccommodationForm)}", navegationParameter);
        }
        else
        {
            await Shell.Current.DisplayAlert("No tan rapido vaquero", "Espera a que se cargue la multimedia para editarla", "ok");
        }
    }

    [RelayCommand]
    private async Task EditAccommodationInformationAsync()
    {
        var navegationParameter = new ShellNavigationQueryParameters
        {
            {"Accommodation", Accommodation },
            {"index", 6 }
        };
        await Shell.Current.GoToAsync($"{nameof(EditAccommodationForm)}", navegationParameter);
    }

    [RelayCommand]
    private async void DeleteAccommodation()
    {
        String meessage;
        try
        {
            bool confirm = await Shell.Current.DisplayAlert("Cuidado","¿Estas Seguro que deseas elimianr el alojamiento, esta accion no se puede desacher","Si, eliminar","No, Cancelar accion");
            if (confirm)
            {
                _accommodationsService = new AccommodationsService();
                meessage = await _accommodationsService.DeleteAccommodation(Accommodation._id);
                _ = Shell.Current.DisplayAlert("Exito", "El alojamiento se ha borrado correctamente, vuelva a cargar sus publicaciones", "OK");
                App.ContentViewHost = new HostAccommodationsView(new AccommodationsOwnedViewModel());
                await Shell.Current.GoToAsync(nameof(HostView));
            }
        }
        catch (Exception ex)
        {
            _ = Shell.Current.DisplayAlert("Lo sentimos", ex.Message, "OK");
        }

    } 



}




