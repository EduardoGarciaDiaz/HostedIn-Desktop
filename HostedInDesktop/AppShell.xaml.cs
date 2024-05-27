﻿using HostedInDesktop.Views;

namespace HostedInDesktop
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(GuestView), typeof(GuestView));
            Routing.RegisterRoute("/Login", typeof(Login));
            Routing.RegisterRoute(nameof(EditProfile), typeof(EditProfile));
            Routing.RegisterRoute(nameof(DeleteAccount), typeof(DeleteAccount));
            Routing.RegisterRoute(nameof(HostView), typeof(HostView));
            Routing.RegisterRoute(nameof(Profile), typeof(Profile));
            Routing.RegisterRoute(nameof(SignupView), typeof(SignupView));
            Routing.RegisterRoute(nameof(AccommodationDetails), typeof(AccommodationDetails));
            Routing.RegisterRoute(nameof(AccommodationForm), typeof(AccommodationForm));
            Routing.RegisterRoute(nameof(EditAccommodationForm), typeof(EditAccommodationForm));
        }
    }
}
