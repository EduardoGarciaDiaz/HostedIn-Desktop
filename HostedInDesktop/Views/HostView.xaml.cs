namespace HostedInDesktop.Views;

public partial class HostView : ContentPage
{
	public HostView()
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