using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using GoogleApi.Entities.Maps.AddressValidation.Response;
using HostedInDesktop.Data.Models;
using HostedInDesktop.Data.Services;
using HostedInDesktop.Messages;
using HostedInDesktop.Utils;
using HostedInDesktop.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostedInDesktop.viewmodels
{
    public partial class ProfileViewModel : ObservableObject
    {
        private readonly IUserService _userService = new UserService();
        private string _userId = App.user._id;

        public ProfileViewModel()
        {
            WeakReferenceMessenger.Default.Register<ProfileMesssage>(this, (r, m) =>
            {
                AdditionalContent = null;
                GetUserById();
            });

            GetUserById();
        }

        [ObservableProperty]
        private bool isLoading;

        [ObservableProperty]
        private string fullname;

        [ObservableProperty]
        private ImageSource profilePhoto;

        [ObservableProperty]
        private bool isButtonEnabled;

        [ObservableProperty]
        private View _additionalContent;

        [RelayCommand]
        public async void MyAccountCliked()
        {
            AdditionalContent = new EditProfile();
            WeakReferenceMessenger.Default.Send(new EditProfileMessage(App.user));
        }

        [RelayCommand]
        public async void DeleteAccountCliked()
        {
            AdditionalContent = new DeleteAccount();
        }

        [RelayCommand]
        public async void ChangePasswordCliked()
        {
            AdditionalContent = new ChangePasswordView();
        }

        [RelayCommand]
        public async void LogoutCliked()
        {
            //TODO: LogOut
        }

        [RelayCommand]
        public async void GoBack()
        {
            App.contentToShow = new ExploreView();
            await Shell.Current.GoToAsync(nameof(GuestView));
        }

        public async Task GetUserById()
        {
            isButtonEnabled = false;

            if (Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
            {
                try
                {
                    IsLoading = true;

                    if (!string.IsNullOrWhiteSpace(_userId))
                    {
                        User user = await _userService.GetUserById(_userId);
                        if (Preferences.ContainsKey(nameof(App.user)))
                        {
                            Preferences.Remove(nameof(App.user));
                        }

                        string userDetails = JsonConvert.SerializeObject(user);
                        App.user = user;
                        ShowUserData();
                        EnableButtons();
                    }
                    else
                    {
                        await Shell.Current.DisplayAlert("Error", "El usuario viene nulo, vuelva a iniciar sesión", "Ok");
                        await Shell.Current.GoToAsync("///Login");
                        return;
                    }
                }
                catch (UnauthorizedAccessException)
                {
                    await Shell.Current.DisplayAlert("La sesión caducó", "La sesión caducó debido a inactividad.", "Ir a inicio de sesión");
                    await Shell.Current.GoToAsync("///Login");
                }
                catch (ApiException aex)
                {
                    Console.WriteLine(aex.StackTrace);
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

        private void ShowUserData()
        {
            User userLoaded = App.user;
            Fullname = userLoaded.fullName;

            if (userLoaded.profilePhoto != null && userLoaded.profilePhoto.data != null && userLoaded.profilePhoto.data.Length > 0)
            {
                byte[] imageData = userLoaded.profilePhoto.data;
                ProfilePhoto = ImageSource.FromStream(() => new MemoryStream(imageData));
            } 
            else
            {
                ProfilePhoto = "ic_user.png";
            }
        }

        private void EnableButtons()
        {
            if (!isButtonEnabled)
            {
                IsButtonEnabled = true;
            }
        }
    }
}
