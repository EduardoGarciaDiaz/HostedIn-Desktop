using Grpc;
using HostedInDesktop.Data.Models;
using HostedInDesktop.Views;

namespace HostedInDesktop
{
    public partial class App : Application
    {

        public static User user;
        public static string token;
        public static string Google_API_Keys = "AIzaSyBMCpitlSaWhnRQ5LNuOeUchSDi9sCO364";
        public static bool hostMode;

        public App()
        {
            InitializeComponent();
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MzI5MTcyNEAzMjM1MmUzMDJlMzBtZ3M3dFBiU1NaMFVvZHBjcU5kL0R1UTVyRk1ZTzlTazZHamhaZzU3SW04PQ==");
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
