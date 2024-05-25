namespace HostedInDesktop.Views;

public partial class GuestView : ContentPage
{
	public GuestView()
	{
		InitializeComponent();
	}

    private void OnEditClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync(nameof(EditProfile));
    }

    private void OnDeleteClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync(nameof(DeleteAccount));
    }
}