using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostedInDesktop.viewmodels
{
    public partial class LoginViewModel : ObservableObject
    {
        [ObservableProperty]
        private string email = "";

        [ObservableProperty]
        private string password = "";

        [RelayCommand]
        public void Login()
        {
            
        }
    }
}
