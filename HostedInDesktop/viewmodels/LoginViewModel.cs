﻿using CommunityToolkit.Mvvm.ComponentModel;
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
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HostedInDesktop.viewmodels
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly IAuthService _authService = new AuthService();

        [ObservableProperty]
        private string email;

        [ObservableProperty]
        private string password;

        [RelayCommand]
        public async void SignIn()
        {
            if (Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
            {
                try
                {
                    if (!string.IsNullOrWhiteSpace(email) && !string.IsNullOrWhiteSpace(password)) 
                    { 
                        User user = await _authService.SignIn(Email, Password);
                        if (Preferences.ContainsKey(nameof (App.user)))
                        {
                            Preferences.Remove(nameof(App.user));
                        }
                        string userDetails = JsonConvert.SerializeObject(user);
                        App.user = user;
                        if (user.roles.Contains("Guest"))
                        {
                            await Shell.Current.GoToAsync(nameof(GuestView));
                        } 
                        else
                        {
                            await Shell.Current.GoToAsync(nameof(HostView));
                        }
                    }
                    else
                    {
                        await Shell.Current.DisplayAlert("Datos faltantes", "Por favor, introduce tu email y contraseña para iniciar sesión", "Ok");
                        return;
                    }
                }
                catch (ApiException aex)
                {
                    await Shell.Current.DisplayAlert("Error", aex.Message, "Ok");
                    return;
                }
                catch (Exception ex)
                {
                    await Shell.Current.DisplayAlert("Error ", ex.Message, email, "Ok");
                    return;
                }
            }
        }
    }
}