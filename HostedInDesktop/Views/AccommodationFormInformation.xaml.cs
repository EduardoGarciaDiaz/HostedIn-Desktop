using System.Text.RegularExpressions;

namespace HostedInDesktop.Views;

public partial class AccommodationFormInformation : ContentView
{
	public AccommodationFormInformation()
	{
		InitializeComponent();
	}

    private void OnTextChanged(object sender, TextChangedEventArgs e)
    {
        var entry = sender as Entry;
        if (entry == null)
            return;

        string newText = e.NewTextValue;
        if (!Regex.IsMatch(newText, "^[0-9]*$"))
        {
            entry.Text = e.OldTextValue;
        }
    }
}