using HostedInDesktop.Data.Models;
using System.Text.RegularExpressions;

namespace HostedInDesktop.Views;

public partial class EditProfile : ContentView
{
	public EditProfile()
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