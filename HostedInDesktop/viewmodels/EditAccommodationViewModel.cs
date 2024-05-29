

using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HostedInDesktop.Data.Models;
using HostedInDesktop.Data.Services;
using HostedInDesktop.Utils;
using HostedInDesktop.Views;
using Newtonsoft.Json;
using SkiaSharp.Views.Maui.Controls;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace HostedInDesktop.viewmodels;

public partial class EditAccommodationViewModel : ObservableObject
{

    public ObservableCollection<ImageSource> MultimediaItems { get; } = new ObservableCollection<ImageSource>();

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
            var imageSource1 = ImageSource.FromStream(() => new MemoryStream(imageBytes1));
            var imageSource2 = ImageSource.FromStream(() => new MemoryStream(imageBytes2));
            var imageSource3 = ImageSource.FromStream(() => new MemoryStream(imageBytes3));
            MultimediaItems.Clear();
            MultimediaItems.Add(imageSource1);
            MultimediaItems.Add(imageSource2);
            MultimediaItems.Add(imageSource3);
            VideoFilePath = await ImageHelper.SaveVideoToFileAsync(videoBytes4);
            

        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
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
    private async Task EditAccommodationTypeAsync(Accommodation accommodation)
    {
        var navegationParameter = new ShellNavigationQueryParameters
        {
            {"Accommodation", Accommodation },
            {"index", 1 }
        };
        await Shell.Current.GoToAsync($"{nameof(EditAccommodationForm)}", navegationParameter);
    }

    [RelayCommand]
    private async Task EditAccommodationUbicationAsync(Accommodation accommodation)
    {
        var navegationParameter = new ShellNavigationQueryParameters
        {
            {"Accommodation", Accommodation },
            {"index", 2 }
        };
        await Shell.Current.GoToAsync($"{nameof(EditAccommodationForm)}", navegationParameter);
    }

    [RelayCommand]
    private async Task EditAccommodationRoomsNumbersAsync(Accommodation accommodation)
    {
        var navegationParameter = new ShellNavigationQueryParameters
        {
            {"Accommodation", Accommodation },
            {"index", 3 }
        };
        await Shell.Current.GoToAsync($"{nameof(EditAccommodationForm)}", navegationParameter);
    }

    [RelayCommand]
    private async Task EditAccommodationServicesAsync(Accommodation accommodation)
    {
        var navegationParameter = new ShellNavigationQueryParameters
        {
            {"Accommodation", Accommodation },
            {"index", 4 }
        };
        await Shell.Current.GoToAsync($"{nameof(EditAccommodationForm)}", navegationParameter);
    }

    [RelayCommand]
    private async Task EditAccommodationMediaAsync(Accommodation accommodation)
    {
        var navegationParameter = new ShellNavigationQueryParameters
        {
            {"Accommodation", Accommodation },
            {"index", 5 }
        };
        await Shell.Current.GoToAsync($"{nameof(EditAccommodationForm)}", navegationParameter);
    }

    [RelayCommand]
    private async Task EditAccommodationInformationAsync(Accommodation accommodation)
    {
        var navegationParameter = new ShellNavigationQueryParameters
        {
            {"Accommodation", Accommodation },
            {"index", 6 }
        };
        await Shell.Current.GoToAsync($"{nameof(EditAccommodationForm)}", navegationParameter);
    }

    [RelayCommand]
    private void DeleteAccommodation(Accommodation accommodation)
    {

    } 


}




