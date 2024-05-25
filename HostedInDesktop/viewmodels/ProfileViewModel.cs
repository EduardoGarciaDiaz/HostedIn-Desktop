using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HostedInDesktop.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostedInDesktop.viewmodels
{
    public partial class ProfileViewModel : ObservableObject
    {
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
            //TODO: AdditionalContent = new ChangePassword();
        }

        [RelayCommand]
        public async void LogoutCliked()
        {
            //TODO: AdditionalContent = new ChangePassword();
        }
    }
}
