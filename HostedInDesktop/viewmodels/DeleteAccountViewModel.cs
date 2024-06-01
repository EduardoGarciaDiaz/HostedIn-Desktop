using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HostedInDesktop.Data.Models;
using HostedInDesktop.Data.Services;
using HostedInDesktop.Utils;
using HostedInDesktop.Views;


namespace HostedInDesktop.viewmodels
{
    public partial class DeleteAccountViewModel : ObservableObject
    {
        private readonly IUserService _userService = new UserService();
        private string userId = App.user._id;


        [ObservableProperty]
        private string fullname;

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
                            if (idDeleted != null)
                            {
                                App.user = null;
                                await Shell.Current.DisplayAlert("Cuenta eliminada", "Tu cuenta ha sido eliminada.", "Ok");
                                await Shell.Current.GoToAsync($"//{nameof(Login)}");
                            }
                        }
                        else
                        {
                            await Shell.Current.DisplayAlert("Contraseña incorrecta", "La contraseña ingresada no coincide con la contraseña actual", "Ok");
                        }
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
            string passwordConfirmation = Password.Trim();
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
