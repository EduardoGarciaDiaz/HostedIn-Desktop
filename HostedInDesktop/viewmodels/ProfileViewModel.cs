using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HostedInDesktop.Data.Models;
using HostedInDesktop.Data.Services;
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
            GetUserById();
        }

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
            AdditionalContent = null;
            await Shell.Current.GoToAsync(nameof(GuestView));
        }

        public async Task GetUserById()
        {
            isButtonEnabled = false;

            if (Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
            {
                try
                {
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
                        await Shell.Current.GoToAsync(nameof(Login));
                        return;
                    }
                }
                catch (ApiException aex)
                {
                    Console.WriteLine(aex.StackTrace);
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
