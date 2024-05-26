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
            accommodation.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(Accommodation.mainImage))
                {
                    UpdateImage(view, accommodation);
                }
            };

            UpdateImage(view, accommodation);
            view.lblTitle.Text = accommodation.title;
			view.lblDescription.Text = accommodation.description;
			view.lblPrice.Text = $"${accommodation.nightPrice} por noche";
		}
	}

    private static void UpdateImage(AccommodationExploreReusable view, Accommodation accommodation)
    {
        if (accommodation.mainImage != null && accommodation.mainImage.Length > 0)
        {
            view.imgAccommodation.Source = ImageSource.FromStream(() => new MemoryStream(accommodation.mainImage));
        }
        else
        {
            view.imgAccommodation.Source = ImageSource.FromFile("img_provisional.png");
        }
    }

}