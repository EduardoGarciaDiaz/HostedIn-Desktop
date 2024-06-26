﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using HostedInDesktop.Messages;
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
            CurrentView = App.contentToShow;
        }

        [RelayCommand]
        public void OnExploreClicked()
        {
            CurrentView = new ExploreView();   
        }

        [RelayCommand]
        public void OnBookingsClicked()
        {
            CurrentView = new BookingsView();
        }

        [RelayCommand]
        public void OnStaticticsClicked()
        {
            CurrentView = new StaticticsView();
        }

        [RelayCommand]
        public void OnChangeToHost()
        {
            App.hostMode = true;
            App.ContentViewHost = new HostBookedAccommodations(new AcoommodationsBookedHostViewModel());
            Shell.Current.GoToAsync(nameof(HostView));
        }

        [RelayCommand]
        public void MyProfileClickedCommand()
        {
            WeakReferenceMessenger.Default.Send(new ProfileMesssage(App.user));
            Shell.Current.GoToAsync(nameof(Profile));
        }

        [RelayCommand]
        public async Task OnLogOutClicked()
        {

            await Shell.Current.GoToAsync("///Login");

        }
    }
}
