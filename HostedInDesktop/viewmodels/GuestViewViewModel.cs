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
    public partial class GuestViewViewModel: ObservableObject
    {
        [ObservableProperty]
        private ContentView _currentView;
        public GuestViewViewModel() 
        {
            CurrentView = new ExploreView();
        }

        [RelayCommand]
        public void OnExploreClicked()
        {
            CurrentView = new ExploreView();   
        }

        [RelayCommand]
        public void OnBookingsClicked()
        {
            CurrentView = new BookingsView(new BookingsGuestViewViewModel());
        }

        [RelayCommand]
        public void OnStaticticsClicked()
        {
            CurrentView = new StaticticsView();
        }

        [RelayCommand]
        public void OnChangeToHost()
        {
            Shell.Current.GoToAsync(nameof(HostView));
        }

        [RelayCommand]
        public void MyProfileClickedCommand()
        {
            Shell.Current.GoToAsync(nameof(Profile));
        }
    }
}
