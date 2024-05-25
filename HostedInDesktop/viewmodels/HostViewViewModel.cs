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
    public partial class HostViewViewModel : ObservableObject
    {
        [ObservableProperty]
        private ContentView _currentView;

        public HostViewViewModel()
        {
            CurrentView = new HostBookedAccommodations();
        }

        [RelayCommand]
        public void OnBookedAccommodationsCliked()
        {
            CurrentView = new HostBookedAccommodations();
        }

        [RelayCommand]
        public void OnAccommodationsClicked()
        {
            CurrentView = new HostAccommodationsView();
        }

        [RelayCommand]
        public void OnStaticticsClicked()
        {
            CurrentView = new StaticticsView();
        }

        [RelayCommand]
        public void OnChangeToGuestClicked()
        {
            Shell.Current.GoToAsync(nameof(GuestView));
        }

        [RelayCommand]
        public void MyProfileClicked()
        {
            Shell.Current.GoToAsync(nameof(Profile));
        }
    }
}
