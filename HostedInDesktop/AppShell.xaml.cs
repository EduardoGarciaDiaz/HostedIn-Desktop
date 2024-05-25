using HostedInDesktop.Views;

namespace HostedInDesktop
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(GuestView), typeof(GuestView));
            Routing.RegisterRoute(nameof(Login), typeof(Login));
        }
    }
}
