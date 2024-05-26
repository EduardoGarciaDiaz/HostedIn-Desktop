using System.Windows.Input;

namespace HostedInDesktop.Reusable;

public partial class AccommodationBasicReusable : ContentView
{
    public static readonly BindableProperty BasicTextProperty =
            BindableProperty.Create(nameof(BasicText), typeof(string), typeof(AccommodationBasicReusable), default(string));

    public static readonly BindableProperty ValueProperty =
        BindableProperty.Create(nameof(Value), typeof(int), typeof(AccommodationBasicReusable), default(int));

    public static readonly BindableProperty IncreaseCommandProperty =
        BindableProperty.Create(nameof(IncreaseCommand), typeof(ICommand), typeof(AccommodationBasicReusable));

    public static readonly BindableProperty DecreaseCommandProperty =
        BindableProperty.Create(nameof(DecreaseCommand), typeof(ICommand), typeof(AccommodationBasicReusable));

    public string BasicText
    {
        get => (string)GetValue(BasicTextProperty);
        set => SetValue(BasicTextProperty, value);
    }

    public int Value
    {
        get => (int)GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    public ICommand IncreaseCommand
    {
        get => (ICommand)GetValue(IncreaseCommandProperty);
        set => SetValue(IncreaseCommandProperty, value);
    }

    public ICommand DecreaseCommand
    {
        get => (ICommand)GetValue(DecreaseCommandProperty);
        set => SetValue(DecreaseCommandProperty, value);
    }

    public AccommodationBasicReusable()
    {
        InitializeComponent();
    }
}