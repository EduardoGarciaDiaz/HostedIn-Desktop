using HostedInDesktop.Data.Models;
using HostedInDesktop.Views;

namespace HostedInDesktop
{
    public partial class App : Application
    {

        public static User user;
        public static string token;
        public static string Google_API_Keys = "AIzaSyBMCpitlSaWhnRQ5LNuOeUchSDi9sCO364";

        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }

        protected override Window CreateWindow(IActivationState activationState)
        {
            var window = base.CreateWindow(activationState);

            const int newWidth = 1400;
            const int newHeight = 800;

            window.Width = newWidth;
            window.Height = newHeight;

            return window;
        }
    }
}
