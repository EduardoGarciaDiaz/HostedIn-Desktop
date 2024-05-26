using HostedInDesktop.Views;

namespace HostedInDesktop
{
    public partial class MainPage : ContentPage
    {
        public ContentView content {  get; set; }

        public MainPage()
        {
            InitializeComponent();
            var mapControl = new Mapsui.UI.Maui.MapControl();
            mapControl.Map?.Layers.Add(Mapsui.Tiling.OpenStreetMap.CreateTileLayer());
            content = mapControl;
            content = this.viewMain;
        }
    }

}
