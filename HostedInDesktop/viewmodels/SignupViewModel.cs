using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HostedInDesktop.Data.Models;
using HostedInDesktop.Data.Services;
using HostedInDesktop.Utils;
using HostedInDesktop.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostedInDesktop.viewmodels
{
    public partial class SignupViewModel : ObservableObject
    {

        private readonly IAuthService _authService = new AuthService();

        [ObservableProperty]
        private User _userData;

        [ObservableProperty]
        private DateTime _date;

        [ObservableProperty]
        private string _passwordConfirmation;

        public SignupViewModel() {
            _userData = new User()
            {
                fullName = "",
                phoneNumber = "",
                email = "",
                password = ""
            };
        }


        [RelayCommand]
        public async  Task OnLoginClicked()
        {
            await Shell.Current.GoToAsync("///Login");
        }

        [RelayCommand]
        public async Task SignUp()
        {
            if (Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
            {
                try
                {
                    if (IsUserDataValid())
                    {
                        UserData.roles = ["Host", "Guest"];

                        User user = await _authService.SignUp(createUser());
                        if (Preferences.ContainsKey(nameof(App.user)))
                        {
                            Preferences.Remove(nameof(App.user));
                        }
                        App.user = user;
                        if (user.roles.Contains("Guest"))
                        {
                            App.hostMode = false;
                            App.contentToShow = new ExploreView();
                            await Shell.Current.GoToAsync(nameof(GuestView));
                        }
                        else
                        {
                            App.hostMode = true;
                            App.ContentViewHost = new HostBookedAccommodations(new AcoommodationsBookedHostViewModel());
                            await Shell.Current.GoToAsync(nameof(HostView));
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

        private User createUser()
        {
            User newUser = new User();
            string birthdateMongoDb = DateFormatterUtils.ConvertDateToMongoDbFormat(Date);
            newUser.email = UserData.email.Trim();
            newUser.fullName = UserData.fullName.Trim();
            if (birthdateMongoDb != null)
            {
                newUser.birthDate = birthdateMongoDb;
            }
            newUser.phoneNumber = UserData.phoneNumber.Trim();
            newUser.password = UserData.password.Trim();
            newUser.roles = ["Host", "Guest"];
            return newUser;
        }

        private bool IsUserDataValid()
        {
            bool isUserDataValid = true;

            if (!IsFullNameValid())
            {
                isUserDataValid = false;
            }
            else if (!IsBirthdateValid())
            {
                isUserDataValid = false;
                Shell.Current.DisplayAlert("Lo sentimos...", $"Debes ser mayor de edad", "Ok");
            }
            else if (!IsPhoneNumberValid())
            {
                isUserDataValid = false;
            } else if (!isEmailValid())
            {
                isUserDataValid = false;
            } else if (!isPasswordValid())
            {
                isUserDataValid = false;
            } else if (!isPasswordConfirmationValid())
            {
                isUserDataValid = false;
            }

            return isUserDataValid;
        }

        private bool IsFullNameValid()
        {
            bool isFullNameValid = true;
            string fullName = UserData.fullName.Trim();

            if (fullName == null || string.IsNullOrEmpty(fullName))
            {
                Shell.Current.DisplayAlert("Nombre obligatorio", "Debes ingresar tu nombre completo", "Ok"); isFullNameValid = false;
            }
            else if (!DataValidator.IsFullNameValid(fullName))
            {
                Shell.Current.DisplayAlert("Nombre completo no válido", "Tu nombre completo debe tener al menos 6 caracteres, solo letras mayúsculas, minúsculas y acentos", "Ok");
                isFullNameValid = false;
            }

            return isFullNameValid;
        }

        private bool IsBirthdateValid()
        {
            bool isBirthdateValid = true;

            // Convert the Date property to string
            string birthdateStr = Date.ToString();

            // Check if the birthdate string is empty or null
            if (string.IsNullOrEmpty(birthdateStr))
            {
                Shell.Current.DisplayAlert("Fecha de nacimiento obligatoria", "Debes ingresar tu fecha de nacimiento", "Ok");
                isBirthdateValid = false;
            }
            else
            {
                try
                {
                    DateTime birthdate;
                    bool isValidDate = DateTime.TryParse(birthdateStr, out birthdate);

                    if (!isValidDate)
                    {
                        Shell.Current.DisplayAlert("Fecha no válida", "El formato de la fecha no es válido", "Ok");
                        isBirthdateValid = false;
                    }
                    else
                    {
                        DateTime minAgeDate = DateTime.Today.AddYears(-18);

                        if (birthdate > minAgeDate)
                        {
                            Shell.Current.DisplayAlert("Edad no válida", "Debes ser mayor de 18 años", "Ok");
                            isBirthdateValid = false;
                        }
                    }
                }
                catch (FormatException)
                {
                    Shell.Current.DisplayAlert("Fecha no válida", "El formato de la fecha no es válido", "Ok");
                    isBirthdateValid = false;
                }
            }

            return isBirthdateValid;
        }



        private bool IsPhoneNumberValid()
        {
            bool isPhoneNumberValid = true;
            string phoneNumber = UserData.phoneNumber.Trim();

            if (string.IsNullOrEmpty(phoneNumber))
            {
                Shell.Current.DisplayAlert("Número de teléfono obligatorio", "Debes ingresar tu número de teléfono", "Ok");
                isPhoneNumberValid = false;
            }
            else if (!DataValidator.IsPhoneNumberValid(phoneNumber))
            {
                Shell.Current.DisplayAlert("Número de teléfono no válido", "Por favor, ingresa un número de teléfono válido (10 números)", "Ok");
                isPhoneNumberValid = false;
            }

            return isPhoneNumberValid;
        }

        private bool isEmailValid()
        {
            bool isValid = true;
            string email = UserData.email.Trim();
            if (string.IsNullOrEmpty(email))
            {
                Shell.Current.DisplayAlert("Correo electrónico obligatorio", "Debes ingresar un correo electrónico", "Ok");
                isValid = false;
            }
            else if (!DataValidator.IsMailValid(email))
            {
                Shell.Current.DisplayAlert("Correo electrónico no valido", "Por favor, ingresa un correo electrónico válido", "Ok");
                isValid = false;
            }
            return isValid;
        }

        private bool isPasswordValid()
        {
            bool isValid = true;
            string password = UserData.password.Trim();
            if (string.IsNullOrEmpty(password))
            {
                Shell.Current.DisplayAlert("Contraseña obligatoria", "Debes ingresar una contraseña", "Ok");
                isValid = false;
            } else if (!DataValidator.IsPasswordValid(password))
            {
                Shell.Current.DisplayAlert("Contraseña no válida", "Por favor, ingresa una contraseña válida", "Ok");
                isValid = false;
            }
            return isValid;
        }

        private bool isPasswordConfirmationValid()
        {
            bool isValid = true;
            if (string.IsNullOrEmpty(PasswordConfirmation))
            {
                Shell.Current.DisplayAlert("Confirmacion de contraseña obligatoria", "Por favor, confirma la contraseña", "Ok");
                isValid = false;
            }
            else if (!PasswordConfirmation.Equals(UserData.password))
            {
                Shell.Current.DisplayAlert("Las contraseñas no coinciden", "Por favor, verifica que las contraseñas coincidan", "Ok");
                isValid = false;
            }
            return isValid;
        }
        

    }
}
