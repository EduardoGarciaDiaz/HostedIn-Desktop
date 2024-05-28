using CommunityToolkit.Mvvm.ComponentModel;

namespace HostedInDesktop.Reusable;

public partial class UserAccommodationReviewReusable : ContentView
{
    public static readonly BindableProperty GuestNameProperty =
            BindableProperty.Create(nameof(GuestName), typeof(string), typeof(UserAccommodationReviewReusable), default(string));

    public static readonly BindableProperty ValueRatingProperty =
        BindableProperty.Create(nameof(ValueRating), typeof(float), typeof(UserAccommodationReviewReusable), default(float));

    public static readonly BindableProperty DescriptionProperty =
        BindableProperty.Create(nameof(Description), typeof(string), typeof(UserAccommodationReviewReusable), default(string));

    public static readonly BindableProperty ProfilePhotoProperty =
       BindableProperty.Create(nameof(ProfilePhoto), typeof(ImageSource), typeof(UserAccommodationReviewReusable), default(ImageSource));

    public string GuestName
    {
        get => (string)GetValue(GuestNameProperty);
        set => SetValue(GuestNameProperty, value);
    }

    public float ValueRating
    {
        get => (float)GetValue(ValueRatingProperty);
        set => SetValue(ValueRatingProperty, value);
    }

    public string Description
    {
        get => (string)GetValue(DescriptionProperty);
        set => SetValue(DescriptionProperty, value);
    }

    public ImageSource ProfilePhoto
    {
        get => (ImageSource)GetValue(ProfilePhotoProperty);
        set => SetValue(ProfilePhotoProperty, value);
    }

    public UserAccommodationReviewReusable()
	{
		InitializeComponent();
	}
}