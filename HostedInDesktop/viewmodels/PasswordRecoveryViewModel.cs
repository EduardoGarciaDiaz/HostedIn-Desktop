using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HostedInDesktop.Data.Services;
using HostedInDesktop.Data.Services.Responses;
using HostedInDesktop.Utils;
using HostedInDesktop.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostedInDesktop.viewmodels
{
    public partial class PasswordRecoveryViewModel : ObservableObject 
    {
        private IUserService _userService = new UserService();

        [ObservableProperty]
        private string email;
        [ObservableProperty]
        private string code;
        [ObservableProperty]
        private string password;

        [ObservableProperty]
        private bool isCodeEnable = false;
        [ObservableProperty]
        private bool isPasswordEnable = false;

        private string token;





        [RelayCommand]
        public async Task OnConfirmEmailAsync()
        {
            try
            {
                if (!String.IsNullOrEmpty(Email))
                {
                    GenericStringClass genericString = new GenericStringClass
                    {
                        content = Email

                    };
                   String message = await  _userService.SendEmailCode(genericString);
                   await Shell.Current.DisplayAlert("Info",$"{message}, Ahora ingresa el codigo de verificación", "Ok");
                   IsCodeEnable = true;
                }
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

        }

        [RelayCommand]
        public async Task OnConfirmCodeAsync()
        {
            try
            {
                if (Code.Length == 5)
                {
                    GenericStringClass genericString = new GenericStringClass
                    {
                        content = Code

                    };
                    token = await _userService.VerifyCode(genericString);
                    await Shell.Current.DisplayAlert("Info", "Codigo correcto, ahora ingresa tu contraseña", "Ok");
                    IsPasswordEnable = true;
                }
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
        }

        [RelayCommand]
        public async void OnConfirmPassword()
        {
            try
            {
                if (!String.IsNullOrEmpty(Password))
                {
                    RecoverPassswordRequest recoverPassswordRequest = new RecoverPassswordRequest
                    {
                        email = this.Email,
                        newPassword = Password
                    };
                    String messsage = await _userService.updatePassword(recoverPassswordRequest, token);
                    await Shell.Current.DisplayAlert("Info", messsage, "Ok");
                    await Shell.Current.GoToAsync("../");
                }
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
        }

        [RelayCommand]
        public async Task GoBack()
        {
            await Shell.Current.GoToAsync("../");
        }

    }
}
