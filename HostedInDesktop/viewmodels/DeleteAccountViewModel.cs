using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HostedInDesktop.Data.Models;
using HostedInDesktop.Data.Services;
using HostedInDesktop.Utils;


namespace HostedInDesktop.viewmodels
{
    public partial class DeleteAccountViewModel : ObservableObject
    {
        private readonly IUserService _userService = new UserService();
        private string userId = App.user._id;

        [ObservableProperty]
        private string password;

        [RelayCommand]
        public async void DeleteAccount()
        {
            if (Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
            {
                try
                {
                    if (IsPasswordConfirmationValid())
                    {
                        if (IsMatchPassword() && userId != null)
                        {
                            string idDeleted = await _userService.DeleteAccount(userId);
                            //await Shell.Current.GoToAsync(nameof(MyProfile));
                        }
                        else
                        {
                            await Shell.Current.DisplayAlert("Contraseña incorrecta", "La contraseña ingresada no coincide con la contraseña actual", "Ok");
                        }
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

        private bool IsPasswordConfirmationValid()
        {
            bool isPasswordConfirmationValid = true;
            string passwordConfirmation = password;

            if (string.IsNullOrEmpty(passwordConfirmation))
            {
                Shell.Current.DisplayAlert("Contraseña obligatoria", "Debes ingresar tu contraseña actual", "Ok");
                isPasswordConfirmationValid = false;
            }

            return isPasswordConfirmationValid;
        }

        private bool IsMatchPassword()
        {
            bool isMatchPassword = false;
            string passwordConfirmation = password.Trim();
            string originalPassword = App.user.password;

            if (!string.IsNullOrEmpty(originalPassword))
            {
                if (BCrypt.Net.BCrypt.Verify(passwordConfirmation, originalPassword))
                {
                    isMatchPassword = true;
                }
            }

            return isMatchPassword;
        }

    }
}
