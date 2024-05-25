namespace HostedInDesktop.Views;

public partial class GuestView : ContentPage
{
	public GuestView()
	{
		InitializeComponent();
		if (!App.user.roles.Contains("Host"))
		{
			RemoveChangeModeItem();
		}
    }

    private void RemoveChangeModeItem()
    {
		var menu = menItemChangeMode;
		if (menu != null)
		{
			Menu.Remove(menu);
		}
    }
}