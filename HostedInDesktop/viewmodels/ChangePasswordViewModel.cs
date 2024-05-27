using BCrypt.Net;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HostedInDesktop.Data.Models;
using HostedInDesktop.Data.Services;
using HostedInDesktop.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostedInDesktop.viewmodels
{
    public partial class ChangePasswordViewModel : ObservableObject
    {
        private readonly IUserService _userService = new UserService();

        [ObservableProperty]
        private string _currentPassword = "";

        [ObservableProperty]
        private string _newPassword = "";

        [ObservableProperty]
        private string _confirmationPassword = "";

        [RelayCommand]
        public async Task OnChangePasswordClicked()
        {
            try
            {
                if (ValidateCurrentPassword() && IsValidPassword() && ArePasswordEquals())
                {
                    User user = await _userService.EditAccount(App.user._id, CreateUser());
                    App.user = user;
                    await Shell.Current.DisplayAlert("Éxito", "Se ha actualizado tu contraseña correctamente", "Ok");
                    CurrentPassword = "";
                    NewPassword = "";
                    ConfirmationPassword = "";
                }
            }
            catch (ApiException ex)
            {
                await Shell.Current.DisplayAlert("Ocurrió un problema", ex.Message, "Aceptar");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Ocurrió un problema", ex.Message, "Aceptar");
            }
        }

        private User CreateUser()
        {
            User user = new User()
            {
                _id = App.user._id,
                birthDate = App.user.birthDate,
                email = App.user.email,
                fullName = App.user.fullName,
                occupation = App.user.occupation,
                password = ConfirmationPassword,
                phoneNumber = App.user.phoneNumber,
                residence = App.user.residence,
                roles = App.user.roles,
            };
            return user;
        }

        private bool ValidateCurrentPassword()
        {
            bool isValid = true;
            if (string.IsNullOrWhiteSpace(CurrentPassword))
            {
                Shell.Current.DisplayAlert("Contraseña actual requerida", "Ingrese su contraseña actual para continuar", "Aceptar");
                isValid = false;
            }
            else
            {
                if (!BCrypt.Net.BCrypt.Verify(CurrentPassword, App.user.password))
                {
                    Shell.Current.DisplayAlert("Contraseña actual incorrecta", "La contraseña actual ingresada no corresponde a su contraseña.", "Aceptar");
                    isValid = false;
                }                    
            }
            return isValid;
        }

        private bool IsValidPassword()
        {
            bool isValid = true;
            if (string.IsNullOrWhiteSpace(NewPassword))
            {
                Shell.Current.DisplayAlert("Contraseña nueva requerida", "Ingrese una contraseña  nueva para continuar", "Aceptar");
                isValid = false;
            } 
            else
            {
                if (!DataValidator.IsPasswordValid(NewPassword))
                {
                    Shell.Current.DisplayAlert("Contraseña nueva no válida", "La contraseña nueva debe tener al menos 8 caracteres, letras minúsculas, mayúsculas, al menos un número y un carácter especial.", "Aceptar");
                    isValid = false;
                }   
            }
            return isValid;
        }

        private bool ArePasswordEquals()
        {
            bool areSame = true;
            if (string.IsNullOrWhiteSpace(ConfirmationPassword))
            {

                Shell.Current.DisplayAlert("Confirmacion de contraseña requerida", "Confirme la contraseña para continuar", "Aceptar");
                areSame = false;
            }
            else
            {
                if (!NewPassword.Equals(ConfirmationPassword))
                {
                    Shell.Current.DisplayAlert("Contraseñas no coinciden", "La contraseña nueva no coincide con la confirmación.", "Aceptar");
                    areSame = false;
                }
            }
            return areSame;
        }
    }
}
