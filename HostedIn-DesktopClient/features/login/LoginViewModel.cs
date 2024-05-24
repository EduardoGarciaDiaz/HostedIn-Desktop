using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostedIn_DesktopClient.features.login
{
    public partial class LoginViewModel : ObservableObject
    {
        [ObservableProperty]
        private int count;

        [RelayCommand]
        public void Sumar()
        {
            Count++;
        }
    }
}
