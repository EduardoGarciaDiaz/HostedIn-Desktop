using System.Windows.Input;

namespace HostedInDesktop.Reusable;

public partial class AccommodationTypeReusable : ContentView
{
    public static readonly BindableProperty TypeProperty =
           BindableProperty.Create(nameof(Type), typeof(string), typeof(AccommodationTypeReusable), default(string));

    public static readonly BindableProperty IconProperty =
        BindableProperty.Create(nameof(Icon), typeof(ImageSource), typeof(AccommodationTypeReusable), default(ImageSource));

    public static readonly BindableProperty CommandProperty =
        BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(AccommodationTypeReusable));


    public string Type
    {
        get => (string)GetValue(TypeProperty);
        set => SetValue(TypeProperty, value);
    }

    public ImageSource Icon
    {
        get => (ImageSource)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    public ICommand Command
    {
        get => (ICommand)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public AccommodationTypeReusable()
    {
        InitializeComponent();
        var tapGestureRecognizer = new TapGestureRecognizer();
        tapGestureRecognizer.Tapped += OnTapped;
        this.GestureRecognizers.Add(tapGestureRecognizer);
    }

    private void OnTapped(object sender, EventArgs e)
    {
        if (Command != null && Command.CanExecute(Type))
        {
            Command.Execute(Type);
        }
    }
}