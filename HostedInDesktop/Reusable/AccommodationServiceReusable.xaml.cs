using BruTile.Wms;
using System.Windows.Input;

namespace HostedInDesktop.Reusable;

public partial class AccommodationServiceReusable : ContentView
{
    public static readonly BindableProperty ServiceNameProperty =
       BindableProperty.Create(nameof(ServiceName), typeof(string), typeof(AccommodationServiceReusable), default(string));


    public static readonly BindableProperty CommandProperty =
        BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(AccommodationServiceReusable));


    public string ServiceName
    {
        get => (string)GetValue(ServiceNameProperty);
        set => SetValue(ServiceNameProperty, value);
    }

    public AccommodationServiceReusable()
    {
        InitializeComponent();
        var tapGestureRecognizer = new TapGestureRecognizer();
        tapGestureRecognizer.Tapped += OnTapped;
        this.GestureRecognizers.Add(tapGestureRecognizer);
    }

    public ICommand Command
    {
        get => (ICommand)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    private void OnTapped(object sender, EventArgs e)
    {
        if (Command != null && Command.CanExecute(ServiceName))
        {
            Command.Execute(ServiceName);
        }
    }

}