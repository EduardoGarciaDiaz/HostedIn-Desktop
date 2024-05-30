    using HostedInDesktop.viewmodels;

    namespace HostedInDesktop.Views;

public partial class AccommodationFormMultimedia : ContentView
{
    public AccommodationFormMultimedia()
    {
        InitializeComponent();
    }

    public AccommodationFormMultimedia(ImageSource image1, ImageSource image2, ImageSource image3, string videoPath)
    {
        InitializeComponent();
        imvMainImage.Source = image1;
        imvSecondImage.Source = image2;
        imvThirdImage.Source = image3;
        imvFourthVideo.Source = videoPath;
    }

}