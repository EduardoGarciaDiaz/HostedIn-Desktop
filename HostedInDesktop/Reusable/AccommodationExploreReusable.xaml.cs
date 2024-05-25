using HostedInDesktop.Data.Models;
using System.Windows.Input;

namespace HostedInDesktop.Reusable;

public partial class AccommodationExploreReusable : ContentView
{
	public static readonly BindableProperty AccommodationProperty =
		BindableProperty.Create(nameof(Accommodation), 
			typeof(Accommodation), 
			typeof(AccommodationExploreReusable), 
			null, 
			propertyChanged: OnAccommodationChanged);

    public static readonly BindableProperty CommandProperty =
            BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(AccommodationExploreReusable));


    public ICommand Command
	{
		get => (ICommand)GetValue(CommandProperty);
		set => SetValue(CommandProperty, value);
	}

	public AccommodationExploreReusable()
	{
        InitializeComponent();
        var tapGestureRecognizer = new TapGestureRecognizer();
        tapGestureRecognizer.Tapped += OnTapped;
        this.GestureRecognizers.Add(tapGestureRecognizer); // Añadir al ContentView

    }

    private void OnTapped(object sender, EventArgs e)
    {
        if (BindingContext is Accommodation accommodation && Command != null && Command.CanExecute(accommodation))
        {
            Command.Execute(accommodation);
        }
    }


    public Accommodation Accommodation
	{
		get => (Accommodation)GetValue(AccommodationProperty);
		set => SetValue(AccommodationProperty, value);
	}

	private static void OnAccommodationChanged(BindableObject bindable, object oldValue, object newValue)
	{
		var view = (AccommodationExploreReusable)bindable;
		if (newValue is Accommodation accommodation)
		{
			if (accommodation.mainImage != null)
			{
				view.imgAccommodation.Source = ImageSource.FromStream(() => new MemoryStream(accommodation.mainImage));
			}
			view.lblTitle.Text = accommodation.title;
			view.lblDescription.Text = accommodation.description;
			view.lblPrice.Text = $"${accommodation.nightPrice} por noche";
		}
	}
}