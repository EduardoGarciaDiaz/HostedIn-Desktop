using HostedInDesktop.Views;

namespace HostedInDesktop
{
    public partial class MainPage : ContentPage
    {
        public ContentView content {  get; set; }

        public MainPage()
        {
            InitializeComponent();
            content = this.viewMain;
        }
    }

}
